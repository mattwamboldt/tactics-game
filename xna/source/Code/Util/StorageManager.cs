using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;
using System.IO;

namespace Board_Game.Code.Util
{
    struct SaveOperation
    {
        public string Data;
        public string FileName;
        public bool IsUserData;
    }

    class StorageManager
    {
        private StorageDevice mSelectedDevice;
        Queue<SaveOperation> mOperations;
        private IAsyncResult mResult;

        public StorageManager()
        {
            mOperations = new Queue<SaveOperation>();
            mSelectedDevice = null;
        }

        public void Save(string fileName, string data, bool isUserData)
        {
            CheckDevice();
                
            SaveOperation op = new SaveOperation();
            op.FileName = fileName;
            op.Data = data;
            op.IsUserData = isUserData;
            mOperations.Enqueue(op);
        }

        private void WriteFile()
        {
            // Open a storage container.
            IAsyncResult result = mSelectedDevice.BeginOpenContainer("Board Game", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = mSelectedDevice.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            SaveOperation operation = mOperations.Dequeue();
            Stream stream;

            //user data is save files
            if (operation.IsUserData)
            {
                stream = container.CreateFile(operation.FileName);
            }
            //non user data is editor files
            else
            {
                // Open the file, creating it if necessary.
                stream = File.Open(operation.FileName, FileMode.Create);
            }

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(operation.Data);
            writer.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }

        private void CheckDevice()
        {
            if (mSelectedDevice == null && mOperations.Count == 0)
            {
                mResult = StorageDevice.BeginShowSelector(null, null);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (mOperations.Count != 0)
            {
                if (mSelectedDevice == null)
                {
                    if (mResult.IsCompleted)
                    {
                        mSelectedDevice = StorageDevice.EndShowSelector(mResult);
                        if (mSelectedDevice == null || !mSelectedDevice.IsConnected)
                        {
                            //alert an error and stop the save flow
                            mOperations.Clear();
                        }

                        mResult = null;
                    }
                }
                //write the file to the device
                else if (mSelectedDevice.IsConnected)
                {
                    WriteFile();
                }
            }
        }
    }
}
