using DevExpress.XtraEditors;
using ERPNext_PowerPlay.Models;
using System;
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
                    Frappe_DocList.FrappeDocList DocList = await new FrappeAPI().GetDocs2Print(ps);
                    if (DocList == null) return;
                    if (DocList.data.Count() > 0) Log.Information("[{0}] Collected {1} Documents in {2}s", ps.ID, DocList.data.Count(), clock.Elapsed.TotalSeconds.ToString());
                    foreach (Frappe_DocList.data fd in DocList.data)
                    {
                        bool processed = await p.PrintDoc(fd, ps);//.Frappe_GetDoc(fd.name, ps);
                        if (processed)
                        {
                            string doctype = ps.DocType.GetAttributeOfType<DescriptionAttribute>().Description;
                            await new FrappeAPI().UpdateCount(string.Format("/api/resource/{0}", doctype), fd);
                            await SaveJob(fd);
                        }
                        else
                        {
                            Log.Warning("Document {0}/{1} failed PrindDoc()!", fd.DocType.ToString(), fd.Name);
                        }
                    }
                    if (DocList.data.Count() > 0) Log.Information("Processed {0} Documents in: {1}s", DocList.data.Count(), clock.Elapsed.TotalSeconds.ToString());
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
