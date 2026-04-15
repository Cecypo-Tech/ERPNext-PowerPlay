using DevExpress.XtraEditors;
using ERPNext_PowerPlay.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using DevExpress.CodeParser;

namespace ERPNext_PowerPlay.Helpers
{
    public class PrintJobHelper
    {
        // Static: shared across all PrintJobHelper instances (frmMain + frmPrintSettings each new one up)
        // "run:{ps.ID}"        — guards against two concurrent runs for the same PrinterSetting
        // "{ps.ID}:{doc.Name}" — guards against the same doc/setting combo being processed twice
        private static readonly ConcurrentDictionary<string, byte> _inFlight = new();

        AppDbContext db = new AppDbContext();
        public PrintJobHelper(AppDbContext dbx)
        {
            db = dbx;
        }
        [STAThread]
        public async void RunPrintJobsAsync()
        {
            try
            {
                var p = new PrintActions();
                Stopwatch clock = Stopwatch.StartNew();

                foreach (PrinterSetting ps in db.PrinterSetting.Where(x => x.Enabled).ToList())
                {
                    string runKey = $"run:{ps.ID}";
                    if (!_inFlight.TryAdd(runKey, 0))
                    {
                        Log.Warning("[{0}] Skipping — PrinterSetting already running on another thread.", ps.ID);
                        continue;
                    }
                    try
                    {
                        Frappe_DocList.FrappeDocList DocList = await new FrappeAPI().GetDocs2Print(ps);
                        if (DocList == null) continue;

                        foreach (Frappe_DocList.data fd in DocList.data)
                        {
                            string docKey = $"{ps.ID}:{fd.Name}";
                            if (!_inFlight.TryAdd(docKey, 0))
                            {
                                Log.Warning("[{0}] Skipping {1} — already in-flight on another thread.", ps.ID, fd.Name);
                                continue;
                            }
                            try
                            {
                                bool processed = await p.PrintDoc(fd, ps);
                                if (processed)
                                {
                                    string doctype = ps.DocType.GetAttributeOfType<DescriptionAttribute>().Description;
                                    await new FrappeAPI().UpdateCount(string.Format("/api/resource/{0}", doctype), fd);
                                    await SaveJob(fd);
                                }
                                else
                                {
                                    Log.Warning("Document {0}/{1} failed PrintDoc()!", fd.DocType.ToString(), fd.Name);
                                }
                            }
                            finally
                            {
                                _inFlight.TryRemove(docKey, out _);
                            }
                        }

                        if (DocList.data.Count() > 0) Log.Information("Processed {0} Documents in: {1}s", DocList.data.Count(), clock.Elapsed.TotalSeconds.ToString());
                    }
                    finally
                    {
                        _inFlight.TryRemove(runKey, out _);
                    }
                }
                clock.Stop();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while running print jobs: {0}", ex.Message);
            }
        }
        [STAThread]
        private async Task<bool> SaveJob(Frappe_DocList.data doc)
        {
            using var context = new AppDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                doc.JobDate = DateTime.Now;
                context.JobHistory.Add(doc);

                await context.SaveChangesAsync();

                await transaction.CreateSavepointAsync("BeforeMoreJobs");

                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while saving print job: {0}", ex.Message);
                // If a failure occurred, we rollback to the savepoint and can continue the transaction
                await transaction.RollbackToSavepointAsync("BeforeMoreJobs");

                // TODO: Handle failure, possibly retry inserting jobs
                Log.Warning("RETRY ???");
                return false;
            }

            //try
            //{
            //    doc.JobDate = DateTime.Now;
            //    db.JobHistory.Add(doc);
            //    //await db.JobHistory.AddAsync(doc);
            //    await db.SaveChangesAsync();

            //    // Commit transaction if all commands succeed, transaction will auto-rollback
            //    // when disposed if either commands fails


            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex, "Error while saving print job: {0}", ex.Message);
            //    return false;
            //}
        }
    }
}
