using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimalChess {
    public partial class Form1 : Form {
        private Box[,] papan = new Box[7, 9];
        private Rectangle[,] displayed = new Rectangle[7, 9];

        private int giliran = 1;
        private Box first;
        private Box.Piece clicked = null;
        private Dictionary<string, Box> around;


        private const int BOX_SIZE = 75;
        private int offset = 20;

        public Form1() {
            InitializeComponent();
            initPapan();
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
            papan[2, 8].trapOwner = 1;
            papan[4, 8].trapOwner = 1;
            papan[3, 7].trapOwner = 1;


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
                        } else {
                            g.DrawImage(Image.FromFile($"img/{current.animal.nama}.png"), offset + x * BOX_SIZE, offset + y * BOX_SIZE, BOX_SIZE, BOX_SIZE);
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
                around = null;
            } else {
                if (clicked != null) {
                    Box second = papan[cX, cY];
                    switch (clicked.strength) {
                        case 0:
                            if (cekGerakanRat(second))
                                gantiGiliran(sender, e);
                            break;
                        case 1: case 2: case 3: case 4:
                            if (cekGerakanHewanBiasa(second))
                                gantiGiliran(sender, e);
                            break;
                        case 5: case 6:
                            if (around.ContainsValue(second)) {
                                if (!second.isWater) {
                                    if (second.animal == null || (second.animal.strength <= first.animal.strength && second.animal.player != first.animal.player)) {
                                        second.removeAnimal();
                                        second.animal = clicked;
                                        clicked = null;
                                        first.animal = null;
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
                                                clicked = null;
                                            }
                                            gantiGiliran(sender, e);
                                            break;
                                        case "bawah":
                                            if (bisaNyebrangKeBawah(cX, cY, second)) {
                                                first.animal = null;
                                                papan[cX, cY + 3].removeAnimal();
                                                papan[cX, cY + 3].animal = clicked;
                                                clicked = null;
                                            }
                                            gantiGiliran(sender, e);
                                            break;
                                        case "kiri":
                                            if (bisaNyebrangKeKiri(cX, cY, second)) {
                                                first.animal = null;
                                                papan[cX - 2, cY].removeAnimal();
                                                papan[cX - 2, cY].animal = clicked;
                                                clicked = null;
                                            }
                                            gantiGiliran(sender, e);
                                            break;
                                        case "kanan":
                                            if (bisaNyebrangKeKanan(cX, cY, second)) {
                                                first.animal = null;
                                                papan[cX + 2, cY].removeAnimal();
                                                papan[cX + 2, cY].animal = clicked;
                                                clicked = null;
                                            }
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

                            }
                            gantiGiliran(sender, e);
                            break;
                    }
                    displayPapan();
                } else if (papan[cX, cY].animal != null && papan[cX, cY].animal.player == giliran) {
                    AmbilAnimal(cX, cY);
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
            return second.animal == null && papan[cX, cY + 1].animal == null && papan[cX, cY + 2].animal == null && 
                (papan[cX, cY + 3].animal == null || (papan[cX, cY + 3].animal.strength <= first.animal.strength && first.animal.player != papan[cX, cY - 3].animal.player));
        }

        private bool bisaNyebrangKeAtas(int cX, int cY, Box second) {
            return second.animal == null && papan[cX, cY - 1].animal == null && papan[cX, cY - 2].animal == null && 
                (papan[cX, cY - 3].animal == null || (papan[cX, cY - 3].animal.strength <= first.animal.strength && first.animal.player != papan[cX, cY - 3].animal.player));
        }

        private bool cekGerakanHewanBiasa(Box second) {
            if (around.ContainsValue(second)) {
                if (!second.isWater) {
                    if (second.animal == null || (second.animal.strength <= first.animal.strength && second.animal.player != first.animal.player)) {
                        second.removeAnimal();
                        second.animal = clicked;
                        clicked = null;
                        first.animal = null;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool cekGerakanRat(Box second) {
            if (around.ContainsValue(second)) {
                if (first.isWater == second.isWater) {
                    if (second.animal == null || ((second.animal.strength == 0 || second.animal.strength == 7) && second.animal.player != first.animal.player)) {
                        second.removeAnimal();
                        second.animal = clicked;
                        clicked = null;
                        first.animal = null;
                        return true;
                    }
                }
                else {
                    if (second.animal == null) {
                        second.removeAnimal();
                        second.animal = clicked;
                        clicked = null;
                        first.animal = null;
                        return true;
                    }
                }
            }
            return false;
        }

        private void AmbilAnimal(int cX, int cY) {
            first = papan[cX, cY];
            clicked = papan[cX, cY].animal;
            around = new Dictionary<string, Box>();
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
        }

        private void gantiGiliran(object sender, EventArgs e) {
            giliran %= 2;
            giliran++;
            displayPapan();
        }
    }
}
