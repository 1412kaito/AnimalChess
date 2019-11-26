using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalChess {
    public class Box {
        public class Piece {
            private string[] DICT_HEWAN = { 
                "rat", "cat", "wolf", "dog", 
                "leopard", "tiger", "lion", "elephant" };
            public string nama;
            public int strength;
            public int player;
            public bool isAlive;
            public Piece(int str, int player) {
                nama = DICT_HEWAN[str];
                this.player = player;
                strength = str;
                isAlive = true;
            }
        }

        public Piece animal;
        public bool isTrap, isDen, isWater;
        public int denOwner;
        public int trapOwner;

        public Box() {
            animal = null;
            isTrap = false;
            isDen = false;
            isWater = false;
        }

        internal void removeAnimal() {
            if (animal != null) {
                animal.isAlive = false;
            }
        }
    }
}
