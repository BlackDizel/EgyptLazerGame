using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byters.GameComponents
{
    class Animation
    {

        int frame_width, frame_height;
        /// <summary>
        /// количество строк
        /// </summary>
        int frame_lines_count;
        /// <summary>
        /// глобальный номер текущего кадра: от 0 до _framesCount-1
        /// </summary>
        public byte curFrameNum;
        /// <summary>
        /// количество кадров в строке
        /// </summary>
        int framesInLine;

        /// <summary>
        /// количество кадров в анимации
        /// </summary>
        byte framesCount;

        //задержки между кадрами анимации
        double delay, startDelay;        
        Texture2D tSource;        
        //public Vector2 vSource;        
        Rectangle curRec;

        public delegate void SomeD();
        public event SomeD SomeEvent;

        public bool AnimationEnd;
        
        /// <summary>
        /// создание анимации с раскадровкой в несколько строк
        /// </summary>
        /// <param name="_tex">текстура анимаций</param>
        /// <param name="_pos">точка отрисовки</param>
        /// <param name="_framesCount">количество кадров в анимации</param>
        /// <param name="_framesInLine">количество кадров в строке</param>
        /// <param name="_delay">количество кадров в строке</param>
        public Animation(Texture2D _tex, byte _framesCount, byte _framesInLine, double _delay)
        {
            frame_lines_count = _framesCount / _framesInLine;
            if (_framesCount % _framesInLine != 0) ++frame_lines_count; //округление в большую сторону
            frame_height = _tex.Height / frame_lines_count;

            createAnim(_tex, _framesCount, _framesInLine, _delay);
        }

        /// <summary>
        /// создание анимации с раскадровкой в несколько строк
        /// </summary>
        /// <param name="_tex">текстура анимаций</param>
        /// <param name="_pos">точка отрисовки</param>
        /// <param name="_framesCount">количество кадров в анимации</param>
        /// <param name="_delay">количество кадров в строке</param>
        public Animation(Texture2D _tex, byte _framesCount, double _delay)
        {

            frame_height = _tex.Height;

            createAnim(_tex, _framesCount, _framesCount, _delay);
        }

        void createAnim(Texture2D _tex, byte _framesCount, byte _framesInLine, double _delay)
        {
            framesInLine = _framesInLine;
            frame_width = _tex.Width / _framesInLine;
            framesCount = _framesCount;
            curFrameNum = 0;
            tSource = _tex;
            delay = _delay;

            curRec = new Rectangle();

            AnimationEnd = false;
        }


        /// <summary>
        /// отрисовка анимации
        /// </summary>
        /// <param name="_sb">целевая группа спрайтов</param>       
        public void Draw(SpriteBatch _sb, Vector2 _pos, double totalMilliseconds)
        {
            prepareDraw(totalMilliseconds);
            _sb.Draw(tSource, _pos, curRec, Color.White);
        }
        public void DrawMirrored(SpriteBatch _sb, Vector2 _pos, double totalMilliseconds)
        {
            prepareDraw(totalMilliseconds);
            _sb.Draw(tSource, _pos, curRec, Color.White, 0, Vector2.Zero,1, SpriteEffects.FlipHorizontally, 0);
        }


        public void Draw(SpriteBatch _sb, Rectangle _recPos, double totalMilliseconds)
        {
            prepareDraw(totalMilliseconds);
            _sb.Draw(tSource, _recPos, curRec, Color.White);
        }

        public void Draw(SpriteBatch _sb, Rectangle _recPos, double totalMilliseconds,float d)
        {
            prepareDraw(totalMilliseconds);
            _sb.Draw(tSource, _recPos, curRec, Color.White, 0, Vector2.Zero, SpriteEffects.None,d);
        }
        

        public void DrawMirrored(SpriteBatch _sb, Rectangle _recPos, double totalMilliseconds)
        {
            prepareDraw(totalMilliseconds);
            _sb.Draw(tSource, _recPos, curRec, Color.White,0,Vector2.Zero,SpriteEffects.FlipHorizontally,0);
        }


        void prepareDraw(double totalMilliseconds)
        {
            curRec.X = frame_width * (curFrameNum % framesInLine);
            curRec.Y = frame_height * (curFrameNum / framesInLine);
            curRec.Width = frame_width;
            curRec.Height = frame_height;

            if (startDelay + delay < totalMilliseconds)
            {
                ++curFrameNum;
                if (curFrameNum == framesCount) if (SomeEvent != null) SomeEvent();// AnimationEnd = true;
                curFrameNum %= framesCount;
                startDelay = totalMilliseconds;
            }
        }        
    }

}
