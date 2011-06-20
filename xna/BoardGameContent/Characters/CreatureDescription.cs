using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Board_Game.Rendering;

namespace Board_Game.Creatures
{
    public enum CreatureType
    {
        Undefined = -1,
        Bomber = 0,
        Fighter = 1,
        Soldier = 2,
        Granadier = 3,
        Miner = 4,
        NumCreatureTypes = 5
    }

    /// <summary>
    /// This class is used to seperate the static data about a Creature
    /// from its runtime data, such as location. That way you can
    /// make a Creature have a certain speed, size, etc. without having to change
    /// All the maps and Creatures lists as well
    /// </summary>
    public class CreatureDescription
    {
        private int mID;
        public int ID
        {
            set { mID = value; }
            get { return mID; }
        }

        private string mName;
        public string Name
        {
            set { mName = value; }
            get { return mName; }
        }

        private string mDescription;
        public string Description
        {
            set { mDescription = value; }
            get { return mDescription; }
        }

        private bool mCanFly;
        public bool CanFly
        {
            set { mCanFly = value; }
            get { return mCanFly; }
        }

        private CreatureType mType;
        public CreatureType Type
        {
            set { mType = value; }
            get { return mType; }
        }

        private Point mSize;
        public Point SizeInSpaces
        {
            set { mSize = value; }
            get { return mSize; }
        }

        private string mTextureName;
        public string TextureName
        {
            set { mTextureName = value; }
            get { return mTextureName; }
        }

        //used to determine which enemies this Creature
        //will attack and in what order
        private CreatureType[] mAttackPriorities;
        public CreatureType[] AttackPriorities
        {
            set { mAttackPriorities = value; }
            get { return mAttackPriorities; }
        }
        
        private Texture2D mTexture;

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return mTexture; }
        }

        public void LoadTexture()
        {
            if (mTexture == null)
            {
                mTexture = TextureManager.Get().Find(mTextureName);
            }
        }

        public bool CanAttack(CreatureType CreatureType)
        {
            return mAttackPriorities != null
                && mAttackPriorities.Contains(CreatureType);
        }
    }

    public class CreatureDescriptionReader : ContentTypeReader<CreatureDescription>
    {
        protected override CreatureDescription Read(
            ContentReader input,
            CreatureDescription existingInstance)
        {
            CreatureDescription desc = new CreatureDescription();
            desc.ID = input.ReadInt32();
            desc.Name = input.ReadString();
            desc.Description = input.ReadString();
            desc.CanFly = input.ReadBoolean();
            desc.Type = input.ReadObject<CreatureType>();
            desc.SizeInSpaces = input.ReadObject<Point>();
            desc.TextureName = input.ReadString();
            desc.AttackPriorities = input.ReadObject<CreatureType[]>();
            desc.LoadTexture();
            return desc;
        }
    }
}
