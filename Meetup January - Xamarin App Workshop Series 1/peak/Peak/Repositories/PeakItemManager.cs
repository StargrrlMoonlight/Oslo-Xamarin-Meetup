using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Peak.Models;

namespace Peak.Repositories
{
    public partial class PeakItemManager
    {
        IMobileServiceSyncTable<PeakItem> peakTable;
        const string offlineDbPath = @"localstore.db";

        public MobileServiceClient CurrentClient { get; }

        private PeakItemManager()
        {
            CurrentClient = new MobileServiceClient(Constants.ApplicationURL);

            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<PeakItem>();

            CurrentClient.SyncContext.InitializeAsync(store);
            peakTable = CurrentClient.GetSyncTable<PeakItem>();
        }

        public static PeakItemManager defaultInstance = new PeakItemManager();
        public static PeakItemManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public bool IsOfflineEnabled
        {
            get { return peakTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<PeakItem>; }
        }

        public async Task<ObservableCollection<PeakItem>> GetPeakItemsAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems) {
                    await SyncPeakItemsAsync();
                }

                IEnumerable<PeakItem> items = await peakTable.ToEnumerableAsync();
                return new ObservableCollection<PeakItem>(items);

            } 
            catch (MobileServiceInvalidOperationException msioe) {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            } 
            catch (Exception e) {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return null;
        }

        public async Task SavePeakItemAsync(PeakItem item)
        {
            if (item.Id == null)
            {
                await peakTable.InsertAsync(item);
            }
            else
            {
                await peakTable.UpdateAsync(item);
            }
        }

        public async Task PushChangesAsync()
        {
            try
            {
                await CurrentClient.SyncContext.PushAsync();

            }
            catch (Exception e)
            {
                Debug.Write(@"SyncService ERROR: {0}", e.Message);
                Debug.Write(@"SyncService ERROR: {0}", @"Swallow the exception as per Azure SDK");
            }
        }

        public async Task SyncPeakItemsAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await CurrentClient.SyncContext.PushAsync();

                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                await peakTable.PullAsync("allPeakItems", peakTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc) {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }

    }
}
