using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AnimalChess {
    public class Box {
        public class Piece {
            private static string[] DICT_HEWAN = { 
                "rat", "cat", "wolf", "dog", 
                "leopard", "tiger", "lion", "elephant" };
            public string nama;
            public int strength;
            public int player;
            public bool isAlive;
            public (int, int) position;
            public Piece(int str, int player) {
                nama = DICT_HEWAN[str];
                this.player = player;
                strength = str;
                isAlive = true;
            }
            override public string ToString() {
                return this.nama + " - " + this.position + " - "+ player;
            }
        }

        public Piece animal;
        public bool isTrap, isDen, isWater;
        public int denOwner = -1;
        public int trapOwner = -1;

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

        public override string ToString() {
            return $"Animal: {this.animal}\nisTrap: {isTrap}\nisDen: {isDen}\nisWater: {isWater}\ntrapOwner:{trapOwner}\ndenOwner:{denOwner}";
        }
    }
}
