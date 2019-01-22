using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class boardForm : Form
    {
        public boardForm()
        {
            InitializeComponent();
        }

        #region Instance Variables
        const int NOT_PICKED_YET = -1;

        int firstCardNumber = NOT_PICKED_YET;
        int secondCardNumber = NOT_PICKED_YET;
        int matches = 0;
        #endregion

        #region Methods

        // This method finds a picture box on the board based on it's number (1 - 20)
        // It takes an integer as it's parameter and returns the picture box controls
        // that's name contains that number
        private PictureBox GetCard(int i)
        {
            PictureBox card = (PictureBox)this.Controls["card" + i];
            return card;
        }

        // This method gets the filename for the image displayed in a picture box given it's number
        // It takes an integer as it's parameter and returns a string containing the 
        // filename for the image in the corresponding picture box
        private string GetCardFilename(int i)
        {
            return GetCard(i).Tag.ToString();
        }

        // This method changes the filename for a given picture box
        // It takes an integer and a string that represents a filename as it's parameters
        // It doesn't return a value but stores the filename for the image to be displayed
        // in the picture box.  It doesn't actually display the new image
        private void SetCardFilename(int i, string filename)
        {
            GetCard(i).Tag = filename;
        }

        // These 2 methods get the value (and suit) of the card in a given picturebox
        // Both methods take an integer as the parameter and return a string

        //gets card value by looking at 5th letter in selected card's filename
        private string GetCardValue(int index)
        {
            return GetCardFilename(index).Substring(4, 1);
        }

        //gets card suit by looking at 6th letter in the selected card's filename
        private string GetCardSuit(int index)
        {
            return GetCardFilename(index).Substring(5, 1);
        }

        private bool IsMatch(int index1, int index2)
        {
            //value1 string is equal to result of calling GetCardValue method on index1
            string value1 = GetCardValue(index1);
            //value2 string is equal to result of calling GetCardValue method on index2
            string value2 = GetCardValue(index2);

            //compares values
            if (value1 != value2)
            {
                return false;
            }
            else return true;
        }

        // This method fills each picture box with a filename
        private void FillCardFilenames()
        {
            string[] values = { "a", "2", "j", "q", "k" };
            string[] suits = { "c", "d", "h", "s" };
            int i = 1;

            for (int suit = 0; suit <= 3; suit++)
            {
                for (int value = 0; value <= 4; value++)
                {
                    SetCardFilename(i, "card" + values[value] + suits[suit] + ".jpg");
                    i++;
                }
            }
        }

        //NOTATE THIS
        private void ShuffleCards()
        {
            Random random = new Random();

            for (int i = 1; i <= 20; i++)
            {
                string currentCardName = GetCardFilename(i);
                int randomCardIndex = random.Next(1, 20);
                string randomCardName = GetCardFilename(randomCardIndex);
                SetCardFilename(i, randomCardName);
                SetCardFilename(randomCardIndex, currentCardName);
            }
        }

        // This method loads (shows) an image in a picture box.  Assumes that filenames
        // have been filled in an earlier call to FillCardFilenames
        private void LoadCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\" + GetCardFilename(i));
        }

        // This method loads the image for the back of a card in a picture box
        private void LoadCardBack(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\black_back.jpg");
        }


        //calls LoadCardBack method for 20 increments, loads card back image in each picture box
        private void LoadAllCardBacks()
        {
            for (int i = 1; i <= 20; i++)
            {
                LoadCardBack(i);
            }
        }

        // Hides a picture box
        private void HideCard(int i)
        {
            GetCard(i).Visible = false;
        }

        //iterates HideCard method until iterator is less than or equal to 20. Hides all cards.
        private void HideAllCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                HideCard(i);
            }
        }

        // shows a picture box
        private void ShowCard(int i)
        {
            LoadCard(i);
        }

        //iterates ShowCard method until iterator is less than or equal to 20. Shows all cards.
        private void ShowAllCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                ShowCard(i);
            }
        }

        // disables a picture box
        private void DisableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Click -= new System.EventHandler(this.card_Click);
        }

        private void DisableAllCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                DisableCard(i);
            }
        }

        private void EnableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Click += new System.EventHandler(this.card_Click);
        }

        private void EnableAllCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                EnableCard(i);
            }
        }

        private void EnableAllVisibleCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                PictureBox card = GetCard(i);
                if (card.Visible)
                {
                    EnableCard(i);
                }
            }
        }

        #endregion

        #region EventHandlers
        private void boardForm_Load(object sender, EventArgs e)
        {
            FillCardFilenames();
            ShuffleCards();
            LoadAllCardBacks();
        }

        private void card_Click(object sender, EventArgs e)
        {
            PictureBox card = (PictureBox)sender;
            int cardNumber = int.Parse(card.Name.Substring(4));

            /* 
             * if the first card isn't picked yet
             *      save the first card index
             *      load the card
             *      disable the card
             *  else (the user just picked the second card)
             *      save the second card index
             *      load the card
             *      disable all of the cards
             *      start the flip timer
             *  end if
            */

            if (firstCardNumber == NOT_PICKED_YET)
            {
                firstCardNumber = cardNumber;
                ShowCard(cardNumber);
                DisableCard(cardNumber);
            }
            else
            {
                secondCardNumber = cardNumber;
                ShowCard(cardNumber);
                DisableAllCards();
                flipTimer.Start();
            }
        }

        private void flipTimer_Tick(object sender, EventArgs e)
        {
            /*
             * stop the flip timer
             * if the first card and second card are a match
             *      increment the number of matches
             *      hide the first card
             *      hide the second card
             *      reset the first card number
             *      reset the second card number
             *      if the number of matches is 10
             *          show a message box
             *      else
             *          enable all of the cards left on the board
             *      end if
             * else
             *      flip the first card back over
             *      flip the second card back over
             *      reset the first card number
             *      reset the second card number
             *      enable all of the cards left on the board
             * end if
             */

            flipTimer.Stop();
            if (IsMatch(firstCardNumber, secondCardNumber))
            {
                matches++;
                HideCard(firstCardNumber);
                HideCard(secondCardNumber);
                firstCardNumber = NOT_PICKED_YET;
                secondCardNumber = NOT_PICKED_YET;

                if (matches == 10)
                {
                    MessageBox.Show("Congratulations, you win!");
                }
                else
                {
                    EnableAllVisibleCards();
                }
            }
            else
            {
                LoadCardBack(firstCardNumber);
                LoadCardBack(secondCardNumber);
                firstCardNumber = NOT_PICKED_YET;
                secondCardNumber = NOT_PICKED_YET;
                EnableAllVisibleCards();
            }

        }
        #endregion
    }
}
