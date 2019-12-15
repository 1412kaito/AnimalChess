using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnimalChess {
    
    public partial class Form1 : Form {
        ComputerPlayer ai, other;
        private Box[,] papan = new Box[7, 9];
        private Rectangle[,] displayed = new Rectangle[7, 9];

        private int giliran = 1;
        private Box first;
        private Box.Piece clicked = null;
        private int callCounter = 0;
        private readonly int DEPTH = 1;
        //private Dictionary<string, Box> around;

        private const int BOX_SIZE = 75;
        private int offset = 20;

        public Form1() {
            InitializeComponent();
            
        }

        void initPapan() {
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 7; x++) {
                    displayed[x, y] = new Rectangle(offset + x * BOX_SIZE, offset + y * BOX_SIZE, BOX_SIZE, BOX_SIZE);
                    Box b = new Box();
                    if (y == 8 || y == 0) {
                        if (x == 2 || x == 4) {
                            b.isTrap = true;
                        }
                        if (x == 3) {
                            b.isDen = true;
                        }
                    } else if (y == 1 || y == 7) {
                        if (x == 3) {
                            b.isTrap = true;
                        }
                    }

                    if (y >= 3 && y <= 5) {
                        if (x == 1 || x == 2 || x == 4 || x == 5) {
                            b.isWater = true;
                        }
                    }
                    papan[x, y] = b;
                }
            }
            papan[3, 0].denOwner = 1;
            papan[3, 8].denOwner = 2;
            papan[2, 0].trapOwner = 1;
            papan[4, 0].trapOwner = 1;
            papan[3, 1].trapOwner = 1; 
            papan[2, 8].trapOwner = 2;
            papan[4, 8].trapOwner = 2;
            papan[3, 7].trapOwner = 2;


            papan[0, 2].animal = new Box.Piece(0, 1);
            papan[5, 1].animal = new Box.Piece(1, 1);
            papan[4, 2].animal = new Box.Piece(2, 1);
            papan[1, 1].animal = new Box.Piece(3, 1);
            
            papan[2, 2].animal = new Box.Piece(4, 1);
            papan[6, 0].animal = new Box.Piece(5, 1);
            papan[0, 0].animal = new Box.Piece(6, 1);
            papan[6, 2].animal = new Box.Piece(7, 1);

            papan[0, 6].animal = new Box.Piece(7, 2);
            papan[5, 7].animal = new Box.Piece(3, 2);
            papan[4, 6].animal = new Box.Piece(4, 2);
            papan[1, 7].animal = new Box.Piece(1, 2); 

            papan[2, 6].animal = new Box.Piece(2, 2);  
            papan[6, 8].animal = new Box.Piece(6, 2);
            papan[0, 8].animal = new Box.Piece(5, 2);
            papan[6, 6].animal = new Box.Piece(0, 2);

            papan[0, 2].animal.position = (0, 2);
            papan[5, 1].animal.position = (5, 1);
            papan[4, 2].animal.position = (4, 2);
            papan[1, 1].animal.position = (1, 1);
            papan[2, 2].animal.position = (2, 2);
            papan[6, 0].animal.position = (6, 0);
            
            papan[0, 0].animal.position = (0, 0);
            
            
            papan[6, 2].animal.position = (6, 2);
            papan[0, 6].animal.position = (0, 6);
            papan[5, 7].animal.position = (5, 7);
            papan[4, 6].animal.position = (4, 6);
            papan[1, 7].animal.position = (1, 7);
            papan[2, 6].animal.position = (2, 6);
            papan[6, 8].animal.position = (6, 8);
            papan[0, 8].animal.position = (0, 8);
            papan[6, 6].animal.position = (6, 6);


            //debugging
            //papan[5,6].animal = new Box.Piece(0, 1);


            //papan[5, 6].animal.position = (5, 6);


            //papan[5, 7].animal = new Box.Piece(5, 2);
            //papan[4, 6].animal = new Box.Piece(5, 2);

            //papan[4, 6].animal.position = (4, 6);
            //papan[5, 7].animal.position = (5, 7);

            displayPapan();
        }

        void displayPapan() {
            panelGame.Invalidate();
            labelGiliran.Text = giliran.ToString();
        }

        private void panelGame_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 7; x++) {
                    Box current = papan[x, y];
                    Brush b;
                    if (current != null) {
                        if (current.isWater) {
                            b = new SolidBrush(Color.Blue);
                        }
                        else if (current.isDen) {
                            b = new SolidBrush(Color.Yellow);
                        }
                        else if (current.isTrap) {
                            b = new SolidBrush(Color.Gray);
                        }
                        else {
                            b = new SolidBrush(Color.Green);
                        }
                        g.FillRectangle(b, offset + x * BOX_SIZE, offset + y * BOX_SIZE, BOX_SIZE, BOX_SIZE);
                        g.DrawRectangle(new Pen(Color.Black), offset + x * BOX_SIZE, offset + y * BOX_SIZE, BOX_SIZE, BOX_SIZE);
                        if (current.animal != null) {
                            if (current.animal.player == 1) {
                                Image img = Image.FromFile($"img/{current.animal.nama}.png");
                                img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                g.DrawImage(img, offset + x * BOX_SIZE, offset + y * BOX_SIZE, BOX_SIZE, BOX_SIZE);
                                
                            }
                            else {
                                g.DrawImage(Image.FromFile($"img/{current.animal.nama}.png"), offset + x * BOX_SIZE, offset + y * BOX_SIZE, BOX_SIZE, BOX_SIZE);
                            }
                        } else {
                        }
                    }
                }
            }
        }

        private void panelGame_MouseClick(object sender, MouseEventArgs e) {
            int cX = -1, cY = -1;
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 7; x++) {
                    Rectangle temp = displayed[x, y];
                    if (temp.Contains(e.Location)) {
                        cX = x; cY = y; break;
                    }
                }
            }

            if (cX == -1 || cY == -1) {
                clicked = null;
            } else {
                if (clicked != null) {
                    Box second = papan[cX, cY];
                    if (second.isDen && second.denOwner == other.giliran) { }
                    else {
                        Tuple<int, int> coordFirst = CoordsOf(papan, first);
                        Dictionary<string, Box> around = AmbilSekeliling(coordFirst.Item1, coordFirst.Item2);
                        switch (clicked.strength) {
                            case 0:
                                if (cekGerakanRat(second, around))
                                    gantiGiliran(sender, e);
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                if (cekGerakanHewanBiasa(second, around))
                                    gantiGiliran(sender, e);
                                break;
                            case 5:
                            case 6:
                                if (around.ContainsValue(second)) {
                                    if (!second.isWater) {
                                        if (second.animal == null || (second.animal.strength <= first.animal.strength && second.animal.player != first.animal.player)) {
                                            second.removeAnimal();
                                            second.animal = clicked;
                                            clicked = null;
                                            first.animal = null;
                                            second.animal.position = CoordsOf(papan, second).ToValueTuple();
                                            gantiGiliran(sender, e);
                                        }
                                    }
                                    else {
                                        string here = around.FirstOrDefault(item => item.Value == second).Key;
                                        switch (here) {
                                            case "atas":
                                                if (bisaNyebrangKeAtas(cX, cY, second)) {
                                                    first.animal = null;
                                                    papan[cX, cY - 3].removeAnimal();
                                                    papan[cX, cY - 3].animal = clicked;
                                                    clicked.position = (cX, cY - 3);
                                                    clicked = null;
                                                }
                                                gantiGiliran(sender, e);
                                                }
                                                break;
                                            case "bawah":
                                                if (bisaNyebrangKeBawah(cX, cY, second)) {
                                                    first.animal = null;
                                                    papan[cX, cY + 3].removeAnimal();
                                                    papan[cX, cY + 3].animal = clicked;
                                                    clicked.position = (cX, cY + 3);
                                                    //MessageBox.Show(clicked.position.ToString());
                                                    clicked = null;
                                                gantiGiliran(sender, e);
                                                }
                                                break;
                                            case "kiri":
                                                if (bisaNyebrangKeKiri(cX, cY, second)) {
                                                    first.animal = null;
                                                    papan[cX - 2, cY].removeAnimal();
                                                    papan[cX - 2, cY].animal = clicked;
                                                    papan[cX - 2, cY].animal.position = (cX - 2, cY);
                                                    clicked = null;
                                                gantiGiliran(sender, e);

                                                }
                                                break;
                                            case "kanan":
                                                if (bisaNyebrangKeKanan(cX, cY, second)) {
                                                    first.animal = null;
                                                    papan[cX + 2, cY].removeAnimal();
                                                    papan[cX + 2, cY].animal = clicked;
                                                    papan[cX + 2, cY].animal.position = (cX + 2, cY);
                                                    //MessageBox.Show(clicked.position.ToString());
                                                    clicked = null;
                                                gantiGiliran(sender, e);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 7:
                                if (around.ContainsValue(second) && !second.isWater && (second.animal == null || (second.animal.strength <= first.animal.strength && second.animal.player != first.animal.player && second.animal.strength != 0))) {
                                    second.removeAnimal();
                                    second.animal = clicked;
                                    clicked = null;
                                    first.animal = null;
                                    second.animal.position = CoordsOf(papan, second).ToValueTuple();
                                }
                                gantiGiliran(sender, e);
                                }
                                break;
                        }
                        displayPapan();
                    }
                } else if (papan[cX, cY].animal != null && papan[cX, cY].animal.player == giliran) {
                    AmbilAnimal(cX, cY);
                }
                Console.WriteLine(papan[cX, cY].ToString());
                if (papan[cX, cY].animal != null) {
                    Console.Write("POSISI TERSIMPAN HEWAN: ");
                    Console.WriteLine(papan[cX, cY].animal.position);
                    Console.WriteLine("INDEX PAPAN: "+(cX, cY));
                    Console.WriteLine(papan[cX, cY].animal.position == (cX, cY));
                }
            }
        }

        private bool bisaNyebrangKeKanan(int cX, int cY, Box second) {
            return second.animal == null && papan[cX + 1, cY].animal == null && (papan[cX + 2, cY].animal == null || (papan[cX + 2, cY].animal.strength <= first.animal.strength && first.animal.player != papan[cX + 2, cY].animal.player));
        }

        private bool bisaNyebrangKeKiri(int cX, int cY, Box second) {
            return second.animal == null && papan[cX - 1, cY].animal == null && (papan[cX - 2, cY].animal == null || (papan[cX - 2, cY].animal.strength <= first.animal.strength && first.animal.player != papan[cX - 2, cY].animal.player));
        }

        private bool bisaNyebrangKeBawah(int cX, int cY, Box second) {
            bool param1 = second.animal == null;
            bool param2 = papan[cX, cY + 1].animal == null;
            bool param3 = papan[cX, cY + 2].animal == null;
            return  param1 && param2 && param3 && (papan[cX, cY + 3].animal == null || papan[cX, cY + 3].animal.strength <= first.animal.strength && first.animal.player != papan[cX, cY + 3].animal.player);
        }

        private bool bisaNyebrangKeAtas(int cX, int cY, Box second) {
            return second.animal == null && papan[cX, cY - 1].animal == null && papan[cX, cY - 2].animal == null && 
                (papan[cX, cY - 3].animal == null || (papan[cX, cY - 3].animal.strength <= first.animal.strength && first.animal.player != papan[cX, cY - 3].animal.player));
        }

        private bool cekGerakanHewanBiasa(Box second, Dictionary<string, Box> around) {
            if (around.ContainsValue(second)) {
                if (!second.isWater) {
                    if (second.animal == null || (second.animal.strength <= first.animal.strength && second.animal.player != first.animal.player)) {
                        second.removeAnimal();
                        second.animal = clicked;
                        second.animal.position = CoordsOf(papan, second).ToValueTuple();
                        clicked = null;
                        first.animal = null;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool cekGerakanRat(Box second, Dictionary<string, Box> around) {
            if (around.ContainsValue(second)) {
                if (first.isWater == second.isWater) {
                    if (second.animal == null || ((second.animal.strength == 0 || second.animal.strength == 7) && second.animal.player != first.animal.player)) {
                        second.removeAnimal();
                        second.animal = clicked;
                        clicked = null;
                        first.animal = null;
                        second.animal.position = CoordsOf(papan, second).ToValueTuple();
                        return true;
                    }
                }
                else {
                    if (second.animal == null) {
                        second.removeAnimal();
                        second.animal = clicked;
                        clicked = null;
                        first.animal = null;
                        second.animal.position = CoordsOf(papan, second).ToValueTuple();
                        return true;
                    }
                }
            }
            return false;
        }

        private Dictionary<string, Box> AmbilSekeliling(int cX, int cY) {
            Dictionary<string, Box> around = new Dictionary<string, Box>();
            if (cY - 1 >= 0) {
                around.Add("atas", papan[cX, cY - 1]);
            }
            if (cY + 1 < 9) {
                around.Add("bawah", papan[cX, cY + 1]);
            }
            if (cX - 1 >= 0) {
                around.Add("kiri", papan[cX - 1, cY]);
            }
            if (cX + 1 < 7) {
                around.Add("kanan", papan[cX + 1, cY]);
            }
            return around;
        }

        private Dictionary<string, (int, int)> AtasBawahKiriKanan((int, int) coords, Box[,] papan) {
            int cX = coords.Item1; int cY = coords.Item2;
            Dictionary<string, (int, int)> around = new Dictionary<string, (int, int)>();
            if (cY - 1 >= 0) {
                around.Add("atas", (cX, cY - 1));
            }
            if (cY + 1 < 9) {
                around.Add("bawah", (cX, cY + 1));
            }
            if (cX - 1 >= 0) {
                around.Add("kiri", (cX - 1, cY));
            }
            if (cX + 1 < 7) {
                around.Add("kanan", (cX + 1, cY));
            }
            return around;
        }

        private void AmbilAnimal(int cX, int cY) {
            first = papan[cX, cY];
            clicked = papan[cX, cY].animal;
        }

        private void gantiGiliran(object sender, EventArgs e) {
            giliran %= 2;
            giliran++;

            if (giliran == ai.giliran) {
                //call minimax
                int depth = DEPTH;
                int playerKe = ai.giliran;
                Box[,] papanBaru = DeepCopy(papan);
                System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
                callCounter = 0;
                Console.WriteLine("START!");
                stopwatch.Start();
                (Move, int) m = MiniMax(papanBaru, depth, playerKe, new Move(papanBaru), int.MinValue, int.MaxValue);
                stopwatch.Stop();
                //do best move
                Move t = m.Item1;
                //if (m.Item2 == int.MaxValue) MessageBox.Show("SAMPAI");
                if (t.next != null) {
                while (t.next.next != null) {
                        Console.WriteLine(t.currentMap);
                    if (t.next != null) {
                        t = t.next;
                    }
                    else {
                        break;
                    }
                }
                }
                if (t != null) {
                    papan = t.currentMap;
                    ambilPiecesYangMasihHidup();
                    giliran %= 2;
                    giliran++;
                }
                Console.WriteLine("TIME:" +stopwatch.Elapsed);
                Console.WriteLine("CALL COUNT:" +callCounter);
            }
            displayPapan();
        }

        Random r = new Random();
        //sisa move, maxim atau minim


        //call MiniMax(map, 3, giliran, moves, int.MinValue, int.MaxValue)

        private (Move, int) MiniMax(Box[,] peta, int depth, int playerSekarang, Move sebelumnya, int alfa, int beta) {
            callCounter++;
            if (depth == 0) {
                int value = 0;

                Box[,] copyPeta = sebelumnya.currentMap;
                List<Box.Piece> myPieces, enemyPieces;
                myPieces = new List<Box.Piece>(); enemyPieces = new List<Box.Piece>();

                for (int y = 0; y < 9; y++) {
                    for (int x = 0; x < 7; x++) {
                        Box currentPiece = copyPeta[x, y];
                        if (currentPiece.animal != null) {
                            if (currentPiece.animal.player == giliran) {
                                myPieces.Add(currentPiece.animal);
                            }
                            else {
                                enemyPieces.Add(currentPiece.animal);
                            }
                        }
                    }
                }

                value += ((myPieces.Count - enemyPieces.Count) * 100);

                int jarakX = int.MinValue, jarakY = int.MinValue;
                int dx = int.MaxValue, dy = int.MaxValue;
                foreach (Box.Piece item in myPieces) {

                    jarakX = 3 - item.position.Item1;
                    jarakX = Math.Abs(jarakX);
                    if (ai.giliran == 1) {
                        jarakY = Math.Abs(8 - item.position.Item2);
                    }
                    else {
                        jarakY = Math.Abs(0 - item.position.Item2);
                    }
                    if (jarakY == jarakX && jarakX == 0) {
                        value = int.MaxValue;
                        return (sebelumnya, value);
                    }
                    else {
                        if (dx > jarakX) dx = jarakX;
                        if (dy > jarakY) dy = jarakY;
                    }
                }

                value += (((jarakX + jarakY) / 2) * -1);

                return (sebelumnya, value);
            }
            else {
                List<Box.Piece> akanGerak = new List<Box.Piece>(); 
                for (int y = 0; y < 9; y++) {
                    for (int x = 0; x < 7; x++) {
                        Box petak = peta[x, y];
                        if (petak.animal != null) {
                            if (petak.animal.isAlive && petak.animal.player == playerSekarang) {
                                akanGerak.Add(petak.animal);
                                ++ctr;
                            }
                        }
                    }
                }

                List<Box[,]> papans = new List<Box[,]>();
                foreach (Box.Piece currentAnimal in akanGerak) {
                    var possibleMoves = AtasBawahKiriKanan(currentAnimal.position, peta);
                    foreach (var move in possibleMoves) {
                        int x = currentAnimal.position.Item1;
                        int y = currentAnimal.position.Item2;
                        if (move.Key.ToLowerInvariant().Equals("bawah".ToLowerInvariant())) {
                            cobaGerakKeBawah(papans, currentAnimal, DeepCopy(peta), x, y);
                        }
                        else if (move.Key.ToLowerInvariant().Equals("kanan".ToLowerInvariant())) {
                            cobaGerakKeKanan(papans, currentAnimal, DeepCopy(peta), x, y);
                        }
                        else if (move.Key.ToLowerInvariant().Equals("atas".ToLowerInvariant())) {
                            cobaGerakKeAtas(papans, currentAnimal, DeepCopy(peta), x, y);
                        }
                        else if (move.Key.ToLowerInvariant().Equals("kiri".ToLowerInvariant())) {
                            cobaGerakKeKiri(papans, currentAnimal, DeepCopy(peta), x, y);
                        }

                    }
                }

                (Move, int) kembalian = default;
                foreach (var item in papans) {
                    Box[,] baru = DeepCopy(item);
                    Move moveSekarang = new Move(baru); moveSekarang.next = sebelumnya;
                    var penampung = MiniMax(baru, depth - 1, (playerSekarang % 2) + 1, moveSekarang, alfa, beta);
                    if (kembalian == default)
                        kembalian = penampung;

                    if (playerSekarang == giliran) {
                        //MAX of MiniMax
                        if (alfa < penampung.Item2) {
                            alfa = penampung.Item2;
                            kembalian = penampung;
                            Console.WriteLine("swap");
                        }
                    }
                    else {
                        //MIN of MiniMax
                        if (beta > penampung.Item2) {
                            beta = penampung.Item2;
                            kembalian = penampung;
                        }
                    }

                    if (alfa >= beta) {
                        break;
                    }
                }
                return kembalian;
            }
        }

        private void cobaGerakKeKiri(List<Box[,]> papans, Box.Piece myAnimal, Box[,] temp, int x, int y) {
            Box cek = temp[x - 1, y];
            if (cek.isDen && cek.denOwner == myAnimal.player) { /*MessageBox.Show("den sendiri left");*/ }
            else {
                if (cek.animal == null && !cek.isWater) {
                    cek.animal = temp[x, y].animal;
                    cek.animal.position = (x - 1, y);
                    temp[x, y].animal = null;
                    papans.Add(temp);
                }
                else {
                    if (cek.animal == null) {
                        if (cek.isWater) {
                            if (myAnimal.strength == 6 || myAnimal.strength == 5) {
                                if (temp[x - 2, y].animal == null) {
                                    if (temp[x - 3, y].animal == null) {
                                        temp[x - 3, y].animal = temp[x, y].animal;
                                        temp[x - 3, y].animal.position = (x - 3, y);
                                        temp[x, y].animal = null;
                                        papans.Add(temp);
                                    }
                                    else
                                    if (temp[x - 3, y].animal.player != myAnimal.player && temp[x - 3, y].animal.strength < myAnimal.strength) {
                                        temp[x - 3, y].removeAnimal();
                                        temp[x - 3, y].animal = temp[x, y].animal;
                                        temp[x - 3, y].animal.position = (x - 3, y);
                                        temp[x, y].animal = null;
                                        papans.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (myAnimal.player != cek.animal.player && cek.isWater == temp[x, y].isWater) {
                            if (cek.isTrap && cek.trapOwner == myAnimal.player) {
                                cek.removeAnimal();
                                cek.animal = myAnimal;
                                temp[x, y].animal = null;

                                myAnimal.position = CoordsOf(temp, cek).ToValueTuple();
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 0 && cek.animal.strength == 7) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x - 1, y);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 7 && cek.animal.strength == 0) {
                                //gaboleh gajah makan tikus
                            }
                            else if (cek.animal.strength <= myAnimal.strength) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x - 1, y);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        private void cobaGerakKeKanan(List<Box[,]> papans, Box.Piece myAnimal, Box[,] temp, int x, int y) {
            Box cek = temp[x + 1, y];
            if (cek.isDen && cek.denOwner == myAnimal.player) { /*MessageBox.Show("den sendiri right"); */}
            else {
                if (cek.animal == null && !cek.isWater) {
                    cek.animal = temp[x, y].animal;
                    cek.animal.position = (x + 1, y);
                    temp[x, y].animal = null;
                    papans.Add(temp);
                }
                else {
                    if (cek.animal == null) {
                        if (cek.isWater) {
                            if (myAnimal.strength == 6 || myAnimal.strength == 5) {
                                if (temp[x + 2, y].animal == null) {
                                    if (temp[x + 3, y].animal == null) {
                                        temp[x + 3, y].animal = temp[x, y].animal;
                                        temp[x + 3, y].animal.position = (x + 3, y);
                                        temp[x, y].animal = null;
                                        papans.Add(temp);
                                    }
                                    else
                                    if (temp[x + 3, y].animal.player != myAnimal.player && temp[x + 3, y].animal.strength < myAnimal.strength) {
                                        temp[x + 3, y].removeAnimal();
                                        temp[x + 3, y].animal = temp[x, y].animal;
                                        temp[x + 3, y].animal.position = (x + 3, y);
                                        temp[x, y].animal = null;
                                        papans.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (myAnimal.player != cek.animal.player && cek.isWater == temp[x, y].isWater) {
                            if (cek.isTrap && cek.trapOwner == myAnimal.player) {
                                cek.removeAnimal();
                                cek.animal = myAnimal;
                                temp[x, y].animal = null;
                                myAnimal.position = CoordsOf(temp, cek).ToValueTuple();
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 0 && cek.animal.strength == 7) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x + 1, y);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 7 && cek.animal.strength == 0) {
                                //gaboleh gajah makan tikus
                            }
                            else if (cek.animal.strength <= myAnimal.strength) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x + 1, y);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        private void cobaGerakKeBawah(List<Box[,]> papans, Box.Piece myAnimal, Box[,] temp, int x, int y) {
            Box cek = temp[x, y + 1];
            if (cek.isDen && cek.denOwner == myAnimal.player) { /*MessageBox.Show("den sendiri bott");*/ }
            else {
                if (cek.animal == null && !cek.isWater) {
                    cek.animal = temp[x, y].animal;
                    cek.animal.position = (x, y + 1);
                    temp[x, y].animal = null;
                    papans.Add(temp);
                }
                else {
                    if (cek.animal == null) {
                        if (cek.isWater) {
                            if (myAnimal.strength == 6 || myAnimal.strength == 5) {
                                if (temp[x, y + 2].animal == null) {
                                    if (temp[x, y + 3].animal == null) {
                                        if (temp[x, y + 4].animal == null) {
                                            temp[x, y + 4].animal = temp[x, y].animal;
                                            temp[x, y + 4].animal.position = (x, y + 4);
                                            temp[x, y].animal = null;
                                            papans.Add(temp);
                                        }
                                        else if (temp[x, y + 4].animal.player != myAnimal.player && temp[x, y + 4].animal.strength < myAnimal.strength) {
                                            temp[x, y + 4].removeAnimal();
                                            temp[x, y + 4].animal = temp[x, y].animal;
                                            temp[x, y + 4].animal.position = (x, y + 4);
                                            temp[x, y].animal = null;
                                            papans.Add(temp);
                                        }
                                    }
                                }
                            }
                        } else {
                            MessageBox.Show("Test");
                        }
                    }
                    else {
                        if (myAnimal.player != cek.animal.player && cek.isWater == temp[x, y].isWater) {
                            if (cek.isTrap && cek.trapOwner == myAnimal.player) {
                                cek.removeAnimal();
                                cek.animal = myAnimal;
                                temp[x, y].animal = null;
                                myAnimal.position = CoordsOf(temp, cek).ToValueTuple();
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 0 && cek.animal.strength == 7) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x, y + 1);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 7 && cek.animal.strength == 0) {
                                //gaboleh gajah makan tikus
                            }
                            else if (cek.animal.strength <= myAnimal.strength) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x, y + 1);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        private void cobaGerakKeAtas(List<Box[,]> papans, Box.Piece myAnimal, Box[,] temp, int x, int y) {
            Box cek = temp[x, y - 1];
            if (cek.isDen && cek.denOwner == myAnimal.player) { /*MessageBox.Show("den sendiri top");*/ }
            else {
                if (cek.animal == null && !cek.isWater) {
                    cek.animal = temp[x, y].animal;
                    cek.animal.position = (x, y - 1);
                    temp[x, y].animal = null;
                    papans.Add(temp);
                }
                else {
                    if (cek.animal == null) {
                        if (cek.isWater) {
                            if (myAnimal.strength == 6 || myAnimal.strength == 5) {
                                if (temp[x, y - 2].animal == null) {
                                    if (temp[x, y - 3].animal == null) {
                                        if (temp[x, y - 4].animal == null) {
                                            temp[x, y - 4].animal = temp[x, y].animal;
                                            temp[x, y - 4].animal.position = (x, y - 4);
                                            temp[x, y].animal = null;
                                            papans.Add(temp);
                                        }
                                        else if (temp[x, y - 4].animal.player != myAnimal.player && temp[x, y - 4].animal.strength < myAnimal.strength) {
                                            temp[x, y - 4].removeAnimal();
                                            temp[x, y - 4].animal = temp[x, y].animal;
                                            temp[x, y - 4].animal.position = (x, y - 4);
                                            temp[x, y].animal = null;
                                            papans.Add(temp);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (myAnimal.player != cek.animal.player && cek.isWater == temp[x, y].isWater) {
                            if (cek.isTrap && cek.trapOwner == myAnimal.player) {
                                cek.removeAnimal();
                                cek.animal = myAnimal;
                                temp[x, y].animal = null;
                                myAnimal.position = CoordsOf(temp, cek).ToValueTuple();
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 0 && cek.animal.strength == 7) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x, y - 1);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                            else if (myAnimal.strength == 7 && cek.animal.strength == 0) {
                                //gaboleh gajah makan tikus
                            }
                            else if (cek.animal.strength <= myAnimal.strength) {
                                cek.removeAnimal();
                                cek.animal = temp[x, y].animal;
                                cek.animal.position = (x, y - 1);
                                temp[x, y].animal = null;
                                papans.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        private Box[,] DeepCopy(Box[, ] ini) {
            Box[,] clones = new Box[7, 9];
            string json = JsonConvert.SerializeObject(ini);
            JArray temp = (JArray)JsonConvert.DeserializeObject(json);
            for (int j = 0; j < 9; j++) {
                for (int i = 0; i < 7; i++) {
                    Box tee = new Box();
                    dynamic current = temp[i][j];
                    tee.isTrap = current.isTrap;
                    tee.isDen = current.isDen;
                    tee.isWater = current.isWater;
                    tee.denOwner = current.denOwner;
                    tee.trapOwner = current.trapOwner;
                    Box.Piece animtee = null;
                    if (current.animal != null) {
                        int str, player;
                        str = current.animal.strength; player = current.animal.player;
                        animtee = new Box.Piece(str, player);
                        animtee.isAlive = current.animal.isAlive;
                        animtee.position = ((int)current.animal.position.Item1, (int)current.animal.position.Item2);
                        tee.animal = animtee;
                    }
                    clones[i, j] = tee;
                }
            }
            return clones;
        }

        Tuple<int, int> CoordsOf<T>(T[,] matrix, T value) {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x) {
                for (int y = 0; y < h; ++y) {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }

        private void atasPVAIToolStripMenuItem_Click(object sender, EventArgs e) {
            initPapan();
            ai = new ComputerPlayer(2);
            ai.pieces = new List<Box.Piece>();
            other = new ComputerPlayer(1);
            other.pieces = new List<Box.Piece>();
            ambilPiecesYangMasihHidup();
            //MessageBox.Show(ai.giliran+"");
        }

        private void ambilPiecesYangMasihHidup() {
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 7; x++) {
                    Box current = papan[x, y];
                    if (current.animal != null) {
                        if (current.animal.isAlive) {
                            if (current.animal.player == ai.giliran) {
                                ai.pieces.Add(current.animal);
                            }
                            if (current.animal.player == other.giliran) {
                                other.pieces.Add(current.animal);
                            }
                        }
                    }
                }
            }
        }

        private void atasAIVPToolStripMenuItem_Click(object sender, EventArgs e) {
            initPapan();
            ai = new ComputerPlayer(1);
            ai.pieces = new List<Box.Piece>();
            other = new ComputerPlayer(2);
            other.pieces = new List<Box.Piece>();
            ambilPiecesYangMasihHidup();
            //MessageBox.Show(ai.giliran + "");

            //call minimax
            int depth = 3;
            int playerKe = ai.giliran;
            Box[,] papanBaru = DeepCopy(papan);
            (Move, int) m = MiniMax(papanBaru, depth, playerKe, new Move(papanBaru), int.MinValue, int.MaxValue);
            //do best move
            Move t = m.Item1;
            //if (m.Item2 == int.MaxValue) MessageBox.Show("SAMPAI");
            while (t.next.next != null) {
                //Console.WriteLine("MAPPPPP");
                //for (int y = 0; y < 9; y++) {
                //    for (int x = 0; x < 7; x++) {
                //        Box current = t.currentMap[x, y];
                //        string character = ".";
                //        if (current.isDen) character = "d";
                //        if (current.isTrap) character = "t";
                //        if (current.isWater) character = "w";
                //        if (current.animal != null) character = current.animal.strength.ToString();
                //        Console.Write(character);
                //    }
                //    Console.WriteLine();
                //}
                if (t.next != null) {
                    t = t.next;
                }
                else {
                    break;
                }
            }
            if (t != null) {
                papan = t.currentMap;
                ambilPiecesYangMasihHidup();
                giliran %= 2;
                giliran++;
            }
        }

        private void redrawToolStripMenuItem_Click(object sender, EventArgs e) {
            panelGame.Invalidate();
            
        }
    }
    class ComputerPlayer {
        public int giliran;
        public List<Box.Piece> pieces;
        public ComputerPlayer(int giliran) {
            this.giliran = giliran;
        }
    }

    class Move {
        public Box[,] currentMap;
        public Move next;
        public Move(Box[, ] map) {
            this.currentMap = map;
        }

    }
}
