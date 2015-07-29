using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EgyptLazerGame.Classes
{
    public class Field
    {
        public static List<Figure> figures;
        public enum FigureStepType { None, Move, Rotate };

        Figure selectedFigure;
        CellObject.Direction direction;
        public bool IsClockwiseRotation;
        public FigureStepType StepType;

        int CurrentPlayer;
        Ray r;

        public Ray RayLight { get { return r; } private set { } }
        public Field()
        {
            figures = new List<Figure>();
            figures.Add(new Figure(Figure.Type.sphinx, new Point(0, 0), CellObject.Direction.Down, 0));
            figures.Add(new Figure(Figure.Type.anubis, new Point(4, 0), CellObject.Direction.Down, 0));
            figures.Add(new Figure(Figure.Type.pharaoh, new Point(5, 0), CellObject.Direction.Down, 0));
            figures.Add(new Figure(Figure.Type.anubis, new Point(6, 0), CellObject.Direction.Down, 0));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(7, 0), CellObject.Direction.Right, 0));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(2, 1), CellObject.Direction.Down, 0));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(0, 3), CellObject.Direction.Up, 0));
            figures.Add(new Figure(Figure.Type.scarab, new Point(4, 3), CellObject.Direction.Left, 0));
            figures.Add(new Figure(Figure.Type.scarab, new Point(5, 3), CellObject.Direction.Up, 0));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(7, 3), CellObject.Direction.Right, 0));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(0, 4), CellObject.Direction.Right, 0));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(7, 4), CellObject.Direction.Up, 0));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(6, 5), CellObject.Direction.Right, 0));

            figures.Add(new Figure(Figure.Type.sphinx, new Point(9, 7), CellObject.Direction.Up, 1));
            figures.Add(new Figure(Figure.Type.anubis, new Point(5, 7), CellObject.Direction.Up, 1));
            figures.Add(new Figure(Figure.Type.pharaoh, new Point(4, 7), CellObject.Direction.Up, 1));
            figures.Add(new Figure(Figure.Type.anubis, new Point(3, 7), CellObject.Direction.Up, 1));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(2, 7), CellObject.Direction.Left, 1));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(7, 6), CellObject.Direction.Up, 1));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(9, 4), CellObject.Direction.Down, 1));
            figures.Add(new Figure(Figure.Type.scarab, new Point(5, 4), CellObject.Direction.Right, 1));
            figures.Add(new Figure(Figure.Type.scarab, new Point(4, 4), CellObject.Direction.Down, 1));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(2, 4), CellObject.Direction.Left, 1));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(9, 3), CellObject.Direction.Left, 1));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(2, 3), CellObject.Direction.Down, 1));
            figures.Add(new Figure(Figure.Type.pyramid, new Point(3, 2), CellObject.Direction.Left, 1));

            CurrentPlayer = 0;
        }

        public void SetSelectedFigure(Point pos)
        {
            var f = (from c in figures where c.Position == pos && c.PlayerID == CurrentPlayer select c).FirstOrDefault();
            if (f != null) selectedFigure = f;
        }

        public void SetDirection(CellObject.Direction dir)
        {
            Figure.DirectionCorrectTest(dir);
            direction = dir;
        }

        public List<CellObject.Direction> IsDirectionsAvailableForSelectedFigure()
        {
            List<CellObject.Direction> lst = new List<CellObject.Direction>();
            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Up)) lst.Add(CellObject.Direction.Up);
            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Right)) lst.Add(CellObject.Direction.Right);
            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Down)) lst.Add(CellObject.Direction.Down);
            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Left)) lst.Add(CellObject.Direction.Left);

            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Up | CellObject.Direction.Left)) lst.Add(CellObject.Direction.Up | CellObject.Direction.Left);
            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Up | CellObject.Direction.Right)) lst.Add(CellObject.Direction.Up | CellObject.Direction.Right);
            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Down | CellObject.Direction.Left)) lst.Add(CellObject.Direction.Down | CellObject.Direction.Left);
            if (IsDirectionAvailableForSelectedFigure(CellObject.Direction.Down | CellObject.Direction.Right)) lst.Add(CellObject.Direction.Down | CellObject.Direction.Right);


            return lst;

        }
        private bool IsDirectionAvailableForSelectedFigure(CellObject.Direction dir)
        {
            if (selectedFigure == null) throw new NullReferenceException("фигура не выбрана");
            if (selectedFigure.FigureType == Figure.Type.sphinx) return false;
            Figure.DirectionCorrectTest(dir);
            Point newPos = Figure.CalcNewPoint(selectedFigure.Position, dir);

            //if cell is not empty and not (figure is scarab and another is pyramid or anubis)
            var f = (from c in figures where c.Position == newPos select c).FirstOrDefault();
            if (f != null && !(selectedFigure.FigureType == Figure.Type.scarab && (f.FigureType == Figure.Type.pyramid || f.FigureType == Figure.Type.anubis)))
                return false;

            //filed borders
            if (newPos.X > 9 || newPos.X < 0 || newPos.Y > 7 || newPos.Y < 0)
                return false;

            //forbiden lines. see game rules
            if (selectedFigure.PlayerID == 0)
                if (newPos.X == 9 || (newPos.X == 1 && (newPos.Y == 0 || newPos.Y == 7)))
                    return false;
            if (selectedFigure.PlayerID == 1)
                if (newPos.X == 0 || (newPos.X == 8 && (newPos.Y == 0 || newPos.Y == 7)))
                    return false;

            return true;
        }

        public bool IsFigureSelected()
        {
            return selectedFigure != null;
        }

        public Point SelectedFigurePosition()
        {
            if (IsFigureSelected())
                return selectedFigure.Position;
            return default(Point);
        }

        /// <summary>
        /// конец хода
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>конец игры?</returns>
        public bool Turn()
        {
            if (StepType == FigureStepType.Move)
            {
                if (selectedFigure.FigureType == Figure.Type.scarab)
                {
                    Point newP = Figure.CalcNewPoint(selectedFigure.Position, direction);
                    var fig = (from c in figures where c.Position.Equals(newP) select c).FirstOrDefault();
                    if (fig != null)
                        fig.Move(CellObject.MirrorDirection(direction));
                }

                if (selectedFigure.FigureType != Figure.Type.sphinx)
                    selectedFigure.Move(direction);
                else return false;

            }
            else
                selectedFigure.Rotate(IsClockwiseRotation);


            var sph = (from c in figures where c.PlayerID == CurrentPlayer && c.FigureType == Figure.Type.sphinx select c).FirstOrDefault();
            if (sph == null) throw new ArgumentNullException();
            r = new Ray(sph.Position, sph.MoveDirection);
            r.Move();
            if (r.IsGameOver) return true;

            ++CurrentPlayer;
            CurrentPlayer %= 2;
            selectedFigure = null;
            StepType = FigureStepType.None;

            return false;
        }

    }
}
