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
    class StorageManager
    {
        private StorageDevice mSelectedDevice;
        private bool mSaveRequested;
        private string mPendingData;
        private string mPendingFile;
        private bool mPendingIsUserData;
        private IAsyncResult mResult;

        public StorageManager()
        {
            mSelectedDevice = null;
            mSaveRequested = false;
            mPendingData = "";
        }

        public void Save(string fileName, string data, bool isUserData)
        {
            //prevents trying to write a file during an existing write request
            if (!Guide.IsVisible && !mSaveRequested)
            {
                mSaveRequested = true;
                mPendingFile = fileName;
                mPendingData = data;
                mPendingIsUserData = isUserData;

                CheckDevice();
            }
        }

        private void WriteFile()
        {
            StorageContainer container = mSelectedDevice.OpenContainer("Board Game");
       
            string filePath = "";
            
            //user data is save files
            if(mPendingIsUserData)
            {
                filePath = Path.Combine(container.Path, mPendingFile);
            }
            //non user data is editor files
            else
            {
                filePath = Path.Combine(StorageContainer.TitleLocation, mPendingFile);
            }

            // Open the file, creating it if necessary.
            FileStream stream = File.Open(filePath, FileMode.Create);

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(mPendingData);
            writer.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }

        private void CheckDevice()
        {
            if (mSelectedDevice == null)
            {
                mResult = Guide.BeginShowStorageDeviceSelector(null, null);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (mSaveRequested)
            {
                if (mSelectedDevice == null)
                {
                    if (mResult.IsCompleted)
                    {
                        mSelectedDevice = Guide.EndShowStorageDeviceSelector(mResult);
                        if (mSelectedDevice == null || !mSelectedDevice.IsConnected)
                        {
                            //alert an error and stop the save flow
                            mSaveRequested = false;
                        }
                    }
                }
                //write the file to the device
                else if (mSelectedDevice.IsConnected)
                {
                    WriteFile();
                    mSaveRequested = false;
                }
            }
        }
    }
}
