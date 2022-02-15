using System;

namespace QRLibrary
{
    internal struct Point
    {
        public int X;
        public int Y;
         public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class QRcode
    {
        public int[,] Matrix { get; private set; }
        public int CorrectionLevel { get; private set; }
        public int VersionNumber { get; private set; }
        public int Side { get; private set; }
        public int Mask { get; private set; }
        public QRcode(string data, int correction_level)
        {
            
                data = Coding.EncodeRequest(data);
                CorrectionLevel = correction_level;
            if (data.Length + 20 <= Tables.MaximumAmountofInformation[correction_level, 39])
            {
                data = Coding.AddingServiceFields(data, correction_level, out int version_number);
                VersionNumber = version_number + 1;
                data = Coding.CreatingCorrectionBytes(Coding.SplittingintoBlocks(data, correction_level, version_number), correction_level, version_number);


                Side = Tables.AlignmentPatterns[VersionNumber][Tables.AlignmentPatterns[VersionNumber].Length - 1] + 7;
                Matrix = new int[Side, Side];
                AddSearchingPatterns();
                if (VersionNumber > 1)
                    AddAlignmentPatterns();
                AddSyncBands();
                if (VersionNumber > 6)
                    AddVersionCode();
                var rnd = new Random();
                Mask = rnd.Next(0, 7);
                AddMaskCode();
                AddInformation(data);
                Side += 8;
                int[,] AddFields = new int[Side, Side];
                DrawSquare(new Point(0, 0), Side, 2, AddFields);
                DrawSquare(new Point(1, 1), Side - 2, 2, AddFields);
                DrawSquare(new Point(2, 2), Side - 4, 2, AddFields);
                DrawSquare(new Point(3, 3), Side - 6, 2, AddFields);
                for (int i = 0; i <= Matrix.GetUpperBound(0); i++)
                {
                    for (int j = 0; j <= Matrix.GetUpperBound(1); j++)
                    {
                        AddFields[i + 4, j + 4] = Matrix[i, j];
                    }
                }
                Matrix = AddFields;
            }
            else
                Mask = -1;
        }

        private void AddInformation(string data)
        {
            int X = Side - 1, Y = Side - 1, counter = 0, status = 0;
            for (int i = 0; i < Side * Side && X>=0; i++)
            {
                if (Matrix[X, Y] == 0)
                {
                    int MaskValue=0;
                    switch (Mask)
                    {
                        case 0:
                            MaskValue = (X + Y) % 2;
                            break;
                        case 1:
                            MaskValue = Y % 2;
                            break;
                        case 2:
                            MaskValue = X % 3;
                            break;
                        case 3:
                            MaskValue = (X + Y) % 3;
                            break;
                        case 4:
                            MaskValue = (X / 3 + Y / 2) % 2;
                            break;
                        case 5:
                            MaskValue = (X * Y) % 2 + (X * Y) % 3;
                            break;
                        case 6:
                            MaskValue = ((X * Y) % 2 + (X * Y) % 3) % 2;
                            break;
                        case 7:
                            MaskValue = ((X * Y) % 3 + (X + Y) % 2) % 2;
                            break;
                    }
                    if (counter < data.Length)
                    {
                        if(MaskValue==0)
                        {
                            if (data[counter] == '1')
                            {
                                Matrix[X, Y] = 2;
                            }
                            else
                            {
                                Matrix[X, Y] = 1;
                            }
                        }
                        else
                        {
                            if (data[counter] == '1')
                            {
                                Matrix[X, Y] = 1;
                            }
                            else
                            {
                                Matrix[X, Y] = 2;
                            }
                        }
                        counter++;
                    }
                    else
                        Matrix[X, Y] = 2;
                }

                switch(status)
                {
                    case 0:
                        status = 1;
                        X--;
                        break;
                    case 1:
                        if(Y-1<0)
                        {
                            X--;
                            status = 2;
                            if (X == 6)
                                X--;
                        }
                        else
                        {
                            X++;
                            Y--;
                            status = 0;
                        }
                        break;
                    case 2:
                        status = 3;
                        X--;
                        break;
                    case 3:
                        if (Y + 1 == Side)
                        {
                            X--;
                            status = 0;
                        }
                        else
                        {
                            X++;
                            Y++;
                            status = 2;
                        }
                        break;
                }
            }
        }

        private void AddMaskCode()
        {
            string maskCode = Tables.MaskCodes[CorrectionLevel, Mask];
            Matrix[8, Side - 8] = 1;
            int x1 = 8, y1 = Side - 1, x2 = 0, y2 = 8;
            for(int i=0;i<6;i++)
            {
                if (maskCode[i] == '1')
                {
                    Matrix[x1, y1] = 1;
                    Matrix[x2, y2] = 1;
                }
                else
                {
                    Matrix[x1, y1] = 2;
                    Matrix[x2, y2] = 2;
                }
                y1--;
                x2++;
            }
            x2++;
            if (maskCode[6] == '1')
            {
                Matrix[x1, y1] = 1;
                Matrix[x2, y2] = 1;
            }
            else
            {
                Matrix[x1, y1] = 2;
                Matrix[x2, y2] = 2;
            }
            x2++;
            x1 = Side - 8;
            y1 = 8;
            for (int i = 7; i < maskCode.Length; i++)
            {
                if (maskCode[i] == '1')
                {
                    Matrix[x1, y1] = 1;
                    Matrix[x2, y2] = 1;
                }
                else
                {
                    Matrix[x1, y1] = 2;
                    Matrix[x2, y2] = 2;
                }
                x1++;
                y2--;
                if (y2 == 6)
                    y2--;
            }
        }

        private void AddVersionCode()
        {
            var strings = Tables.VersionCodes[VersionNumber];
            int x = Side - 11;
            int y = 0;
            for (int i = 0; i < strings.Length; i++)
            {
                var corrent_string = strings[i];
                for (int j = 0; j < corrent_string.Length; j++)
                {
                    if (corrent_string[j] == '1')
                    {
                        Matrix[x, y] = 1;
                        Matrix[y, x] = 1;
                    }
                    else
                    {
                        Matrix[x, y] = 2;
                        Matrix[y, x] = 2;
                    }
                    y++;
                }
                y = 0;
                x++;
            }
        }

        private void AddSyncBands()
        {
            int i = 0, x = 6, y = 6;
            bool corrent=true;
            while (Matrix[x + i + 1, y] == 0 || Matrix[x + i + 1, y] != Matrix[x + i, y])
            {
                if (corrent)
                {
                    Matrix[x + i + 1, y] = 2;
                    Matrix[x, y + i + 1] = 2;
                }
                else
                {
                    Matrix[x + i + 1, y] = 1;
                    Matrix[x, y + i + 1] = 1;
                }
                i++;
                corrent = !corrent;
            }
        }

        public void Print()
        {
            for (int i = 0; i < Side; i++)
            {
                for (int j = 0; j < Side; j++)
                    Console.Write(Matrix[i, j]);
                Console.WriteLine();
            }            
        }

        private void AddSearchingPatterns()
        {
            CreateSearchingPattern(new Point(0, 0));
            CreateSearchingPattern(new Point(0, Side - 7));
            CreateSearchingPattern(new Point(Side - 7, 0));
        }

        private void AddAlignmentPatterns()
        {
            var dots = Tables.AlignmentPatterns[VersionNumber];
            for(int i=0;i<dots.Length;i++)
                for (int j = 0; j < dots.Length; j++)
                {
                    CreateAlignmentPattern(new Point(dots[i], dots[j]));
                }    
        }

        private void CreateAlignmentPattern(Point point)
        {
            if (Matrix[point.X, point.Y] == 0)
            {
                DrawSquare(new Point(point.X - 1, point.Y - 1), 3, 2, Matrix);
                DrawSquare(new Point(point.X - 2, point.Y - 2), 5, 1, Matrix);
                Matrix[point.X, point.Y] = 1;
            }
        }

        private void CreateSearchingPattern(Point start)
        {
            DrawSquare(start, 7, 1, Matrix);
            DrawSquare(new Point(start.X + 1, start.Y + 1), 5, 2, Matrix);
            DrawSquare(new Point(start.X + 2, start.Y + 2), 3, 1, Matrix);
            Matrix[start.X + 3, start.Y + 3] = 1;
            if (start.X == start.Y)
                for (int i = 0; i < 8; i++)
                {
                    Matrix[start.X + i, start.Y + 7] = 2;
                    Matrix[start.X + 7, start.Y + i] = 2;
                }
            else if (start.X < start.Y)
            {
                for (int i = 0; i < 7; i++)
                {
                    Matrix[start.X + i, start.Y - 1] = 2;
                    Matrix[start.X + 7, start.Y + i] = 2;
                }
                Matrix[start.X + 7, start.Y - 1] = 2;
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    Matrix[start.X + i, start.Y + 7] = 2;
                    Matrix[start.X - 1, start.Y + i] = 2;
                }
                Matrix[start.X - 1, start.Y + 7] = 2;
            }
        }

        private void DrawSquare(Point start,int length, int colour, int[,] matrix)
        {
            for (int i = 0; i < length; i++)
            {
                matrix[start.X + i, start.Y] = colour;
                matrix[start.X, start.Y + i] = colour;
                matrix[start.X + i, start.Y + length - 1] = colour;
                matrix[start.X + length - 1, start.Y + i] = colour;
            }
        }
    }
}
