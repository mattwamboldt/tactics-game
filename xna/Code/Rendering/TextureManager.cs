﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Board_Game.Code.Rendering
{
    //controls loading and lookup of textures
    class TextureManager
    {
        Hashtable mTextureCollection;
        ContentManager mContentManager;

        private static TextureManager mInstance;

        public static void Initialize(ContentManager content) { mInstance = new TextureManager(content); }
        public static TextureManager Get() { return mInstance; }

        public TextureManager(ContentManager content)
        {
            mContentManager = content;
            mTextureCollection = new Hashtable();
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
