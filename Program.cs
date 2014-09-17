using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1DV402.S1.L01C
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare variables.
            double subtotal;
            uint cash;
            uint roundedTotalCost;
            double roundingOffAmount;
            uint total;
            uint change;
            uint[] denominations;
            const int CONSOLE_WIDTH = 64;
            const int CONSOLE_HEIGHT = 24;

            // Declare denominations.
            uint[] denominationValuesArray = { 500, 100, 50, 20, 10, 5, 1 };

            // Declare demonation notes.
            string[] denominationNotesArray = new string[]
            { 
                Properties.Resources.denomination500,
                Properties.Resources.denomination100,
                Properties.Resources.denomination50,
                Properties.Resources.denomination20,
                Properties.Resources.denomination10,
                Properties.Resources.denomination5,
                Properties.Resources.denomination1
            };

            // Set title of console screen.
            Console.Title = Properties.Resources.applicationTitle;

            // Make the console a little smaller than usual. No need to take up too much screen space.
            Console.SetWindowSize(CONSOLE_WIDTH, CONSOLE_HEIGHT);

            // Do until user presses Esc key.
            do
            {
                // Clear the console, in case there is output from last loop.
                Console.Clear();

                // Get total cost.
                subtotal = ReadPositiveDouble(Properties.Resources.userReadPositiveDoubleMsg);

                // Get recieved amount.
                cash = ReadUint(Properties.Resources.userReadUintMsg, (uint)subtotal);

                // Declare rounded totalcost variable.
                roundedTotalCost = (uint)Math.Round(subtotal);

                // Count rounding off amount with 2 decimals.
                roundingOffAmount = Math.Round(roundedTotalCost - subtotal, 3);

                // Figure out the amount the customer should pay.
                total = (uint)(subtotal - roundingOffAmount);

                // Figure out how much the customer will recieve back.
                change = cash - total;

                // Get an array with money denominations which should be given back to customer.
                denominations = SplitIntoDenominations(change, denominationValuesArray);

                // Display receipt.
                ViewReceipt(subtotal, roundingOffAmount, total, cash, change, denominationNotesArray, denominations);

                // Inform user that he can run the application again or close it with the Esc key.
                ViewMessage(Properties.Resources.escapeOrResumeMsg);

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape); // Loop until user presses the Esc key.
        }

        /// <summary>
        /// Get and parse user input for total cost.
        /// </summary>
        /// <param name="userInputMessage">Message displayed for user.</param>
        /// <returns>Parsed total cost.</returns>
        static private double ReadPositiveDouble(string userInputMessage)
        {
            // Declare variables
            double parsedDouble;
            double roundedDouble;
            const uint MIN_VALUE = 1;
            string promtedValue;

            // Loop until the user gets the input right.
            while (true)
            {
                // Try to get user input.
                try
                {
                    // Ask the user to enter input for sum.
                    Console.Write(String.Format("\n {0,-20} {1,-2}", userInputMessage, ":"));

                    // Get user input.
                    promtedValue = Console.ReadLine();

                    // Try to parse user input.
                    if (!double.TryParse(promtedValue, out parsedDouble))
                    {
                        // Parse failed. Throw exception with a custom message.
                        throw new FormatException(String.Format("\"{0}\" {1}", promtedValue, Properties.Resources.userInputCouldNotParseMsg));
                    }

                    // Check that the parsed value is higher than the lowest acceptable value.
                    else if (parsedDouble < MIN_VALUE)
                    {
                        // Throw exception with a custom message.
                        throw new OverflowException(String.Format("{0} {1}", parsedDouble, Properties.Resources.userInputValueTooSmallMsg));
                    }

                    // Round the user input.
                    roundedDouble = Math.Round(parsedDouble, 2);

                    // All seems right. Break loop.
                    break;
                }
                // Catch errors.
                catch (Exception exception)
                {
                    // There was something wrong with the users input. Display a message for the user.
                    ViewMessage(String.Format("{0} {1}", Properties.Resources.userInputErrorMsg, exception.Message), true);
                }
            }

            // Return value.
            return roundedDouble;
        }

        /// <summary>
        /// Get and parse user input for recieved ammount.
        /// </summary>
        /// <param name="userInputMessage">Message displayed for user.</param>
        /// <param name="minValue">Minimum value for recived ammount.</param>
        /// <returns>Parsed recieved ammount.</returns>
        static private uint ReadUint(string userInputMessage, uint minValue)
        {
            // Declare variables.
            uint parsedUint;
            string promtedValue;

            // Loop until the user gets the input right.
            while (true)
            {
                // Try to get user input.
                try
                {
                    // Ask the user to enter input for sum.
                    Console.Write(String.Format(" {0,-20} {1,-2}", userInputMessage, ":"));

                    // Get user input
                    promtedValue = Console.ReadLine();

                    // Try to parse user input
                    if (!uint.TryParse(promtedValue, out parsedUint))
                    {
                        // Parse failed. Throw exception with a custom message.
                        throw new FormatException(String.Format("\"{0}\" {1}", promtedValue, Properties.Resources.userInputCouldNotParseMsg));
                    }

                    // Check that the parsed value is higher than the lowest acceptable value.
                    else if (parsedUint < minValue)
                    {
                        // Throw exception with a custom message.
                        throw new OverflowException(String.Format("{0} {1}", parsedUint, Properties.Resources.userInputValueTooSmallMsg));
                    }

                    // Now the user has done everything right. Break this loop.
                    break;
                }
                // Catch errors.
                catch (Exception exception)
                {
                    // Custom thrown (known) errors messages and automated generated error messages are displayed nicely for the user.
                    ViewMessage(String.Format("{0} {1}", Properties.Resources.userInputErrorMsg, exception.Message), true);
                }
            }

            // Return value.
            return parsedUint;
        }

        /// <summary>
        /// Figure out how many money denominations which should be given back to customer.
        /// </summary>
        /// <param name="change">Ammount of change to split into dominations.</param>
        /// <param name="denominations">The different dominations to split the change into.</param>
        /// <returns>Dominations that the custormer should recieve.</returns>
        static private uint[] SplitIntoDenominations(uint change, uint[] denominations)
        {
            // Declare arrays. 
            uint[] denominationReturnArray = new uint[denominations.Length];
            uint arrayKey = 0;

            // Loop through denomination and populate return array with correct number of denominations.
            foreach (uint denomination in denominations)
            {
                // Values less than 1 disappear because return array is of type uint
                denominationReturnArray[arrayKey] = (change / denomination);

                // Calculate whats left of the change
                change %= denomination;

                // Next denomination return array key
                arrayKey++;
            }

            // Return denomination array.
            return denominationReturnArray;
        }

        /// <summary>
        /// Format and disply message for user.
        /// </summary>
        /// <param name="message">Text message to display.</param>
        /// <param name="isError">If this is an error message.</param>
        static private void ViewMessage(string message, bool isError = false)
        {
            // Set white textcolor
            Console.ForegroundColor = ConsoleColor.White;

            // Set red or green backgroundcolor of the console depending on if an error occured or not.
            if (isError)
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }

            // Display message for user.
            Console.WriteLine("\n{0}\n", message);

            // Reset terminal colors.
            Console.ResetColor();
        }

        /// <summary>
        /// Display a receipt for the user.
        /// </summary>
        /// <param name="subtotal">Total cost before rounding.</param>
        /// <param name="roundingOffAmount">Ammount which is rounded.</param>
        /// <param name="total">Total cost for the customer to pay.</param>
        /// <param name="cash">How much the customer paid.</param>
        /// <param name="change">How much the custormer should recieve back.</param>
        /// <param name="notes">Names of denominations notes and coins.</param>
        /// <param name="denominations">Ammount of denominations to recieve.</param>
        static private void ViewReceipt(double subtotal, double roundingOffAmount, uint total, uint cash, uint change, string[] notes, uint[] denominations)
        {
            // Clear screen, for a "more fluent user experience".
            Console.Clear();

            // Print out a nice receipt.
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine(" {0}", Properties.Resources.receiptHeader);
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine(String.Format(" {0,-20}  {1,-5}  {2,11} {3}", Properties.Resources.receiptSubtotal, "|", subtotal, Properties.Resources.receiptCurrency));
            Console.WriteLine(String.Format(" {0,-20}  {1,-5}  {2,11} {3}", Properties.Resources.receiptRoundingOffAmount, "|", roundingOffAmount, Properties.Resources.receiptCurrency));
            Console.WriteLine(String.Format(" {0,-20}  {1,-5}  {2,11} {3}", Properties.Resources.receiptTotal, "|", total, Properties.Resources.receiptCurrency));
            Console.WriteLine(String.Format(" {0,-20}  {1,-5}  {2,11} {3}", Properties.Resources.receiptCash, "|", cash, Properties.Resources.receiptCurrency));
            Console.WriteLine(String.Format(" {0,-20}  {1,-5}  {2,11} {3}", Properties.Resources.receiptChange, "|", change, Properties.Resources.receiptCurrency));
            Console.WriteLine("---------------------------------------------\n");

            // Print out demonations.
            Console.WriteLine(" {0}\n", Properties.Resources.receiptMoneyBack);
            for (uint i = 0; i < denominations.Length; i++)
            {
                // Only print out denomination if needed. When there's more than 0 of the current denomination.
                if (denominations[i] > 0)
                {
                    Console.WriteLine(String.Format(" {0,-15}  {1,-5}  {2,8} {3,0}", notes[i], "|", denominations[i], Properties.Resources.receiptPieces));
                }
            }

            // We need a extra empty line.
            Console.WriteLine();
        }
    }
}
