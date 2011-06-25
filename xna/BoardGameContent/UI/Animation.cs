using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoardGameContent.UI
{
    //This class stores frames and is responsible for interpolating between them
    public class Animation
    {
        private int mCurrentKeyFrame;
        [ContentSerializerIgnore]
        public int CurrentKeyFrame { get { return mCurrentKeyFrame; } set { mCurrentKeyFrame = value; } }

        private ShapeState mCurrentFrame;
        [ContentSerializerIgnore]
        public ShapeState CurrentFrame { get { return mCurrentFrame; } set { mCurrentFrame = value; } }

        protected ShapeState[] mKeyFrames; //Defines position as an offset from the parent
        public ShapeState[] KeyFrames{ get { return mKeyFrames; } set { mKeyFrames = value; } }

        public void Reset()
        {
            CurrentFrame = new ShapeState(KeyFrames[0]);
            CurrentKeyFrame = 0;
        }

        /// <summary>
        /// Calculates the next frame based on the current frame and next keyframe
        /// </summary>
        public void NextFrame()
        {
            int nextKeyFrameIndex = CurrentKeyFrame + 1;
            
            // we hit the end of the animation so stop here
            if (nextKeyFrameIndex < mKeyFrames.Length)
            {
                ShapeState nextKeyFrame = mKeyFrames[nextKeyFrameIndex];
                int framesBetween = nextKeyFrame.Frame - mCurrentFrame.Frame;

                if (framesBetween == 0)
                {
                    mCurrentFrame = new ShapeState(nextKeyFrame);
                }
                else
                {
                    mCurrentFrame.Position = CalculateVectorChange(mCurrentFrame.Position, nextKeyFrame.Position, framesBetween);
                    mCurrentFrame.Size = CalculateVectorChange(mCurrentFrame.Size, nextKeyFrame.Size, framesBetween);
                    mCurrentFrame.Color = CalculateColorChange(mCurrentFrame.Color, nextKeyFrame.Color, framesBetween);
                    mCurrentFrame.Frame += 1;
                }
            }
        }

        private Color CalculateColorChange(Color start, Color end, int framesBetween)
        {
            //may want to Lerp later but seems fine for now
            Color returnColor = start;
            returnColor.A += (byte)((end.A - start.A) / framesBetween);
            returnColor.R += (byte)((end.R - start.R) / framesBetween);
            returnColor.G += (byte)((end.G - start.G) / framesBetween);
            returnColor.B += (byte)((end.B - start.B) / framesBetween);
            return returnColor;
        }

        private Vector2 CalculateVectorChange(Vector2 start, Vector2 end, int framesBetween)
        {
            Vector2 returnValue = start;
            returnValue.X += (end.X - start.X) / framesBetween;
            returnValue.Y += (end.Y - start.Y) / framesBetween;
            return returnValue;
        }
    }

    public class AnimationReader : ContentTypeReader<Animation>
    {
        protected override Animation Read(ContentReader input, Animation existingInstance)
        {
            Animation anim = new Animation();

            anim.KeyFrames = input.ReadObject<ShapeState[]>();
            anim.Reset();

            return anim;
        }
    }
}
