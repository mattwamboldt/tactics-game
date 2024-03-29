﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Board_Game.Rendering
{
    //controls loading and lookup of textures
    public class TextureManager
    {
        Hashtable mTextureCollection;
        ContentManager mContentManager;

        private static TextureManager mInstance;

        public static void Initialize(
            ContentManager content,
            GraphicsDevice device)
        {
            mInstance = new TextureManager(content, device);
        }

        public static TextureManager Get() { return mInstance; }

        public TextureManager(ContentManager content, GraphicsDevice device)
        {
            mContentManager = content;
            mTextureCollection = new Hashtable();

            Texture2D pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            mTextureCollection.Add("RAW", pixel);
        }

        public List<string> GetTextureList()
        {
            return mTextureCollection.Keys.Cast<string>().ToList();
        }

        public Texture2D Find(string textureName)
        {
            Texture2D returnReference = (Texture2D)mTextureCollection[textureName];

            //not loaded yet
            if (returnReference == null)
            {
                returnReference = mContentManager.Load<Texture2D>(textureName);
                if (returnReference == null)
                {
                    Console.WriteLine("Texture " + textureName + " could not be found!");
                }
                else
                {
                    mTextureCollection.Add(textureName, returnReference);
                }
            }

            return returnReference;
        }
    }
}
