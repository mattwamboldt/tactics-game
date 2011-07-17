using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
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
            //prevents trying to write a file during an existing write request
            if (!Guide.IsVisible)
            {
                CheckDevice();
                
                SaveOperation op = new SaveOperation();
                op.FileName = fileName;
                op.Data = data;
                op.IsUserData = isUserData;
                mOperations.Enqueue(op);
            }
        }

        private void WriteFile()
        {
            StorageContainer container = mSelectedDevice.OpenContainer("Board Game");
            SaveOperation operation = mOperations.Dequeue();
       
            string filePath = "";
            
            //user data is save files
            if (operation.IsUserData)
            {
                filePath = Path.Combine(container.Path, operation.FileName);
            }
            //non user data is editor files
            else
            {
                filePath = Path.Combine(StorageContainer.TitleLocation, operation.FileName);
            }

            // Open the file, creating it if necessary.
            FileStream stream = File.Open(filePath, FileMode.Create);

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
                mResult = Guide.BeginShowStorageDeviceSelector(null, null);
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
                        mSelectedDevice = Guide.EndShowStorageDeviceSelector(mResult);
                        if (mSelectedDevice == null || !mSelectedDevice.IsConnected)
                        {
                            //alert an error and stop the save flow
                            mOperations.Clear();
                        }
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
