using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Palindrome_Detector
{
    class PalindromeChecker
    {
        #region fields

        private int minLength;
        private int maxLength;
        private bool includeSpaces;
        private List<string> listOfPalindromes = new List<string>();
        private string[] punctuations = {" ",".", ",", "،", "、","'","\"","‘","’","“","”",":",";","!","?",
            "‹","›","«","»","‐","_","‒","–","—","―","\\","/","⁄","|","#","~","@"};

        #endregion

        #region constructor

        /// <summary>
        /// Constructs a PalindromeChecker object with the given parameters
        /// </summary>
        /// <param name="minLength">The minimum length of a palindrome to be detected</param>
        /// <param name="maxLength">The maximum length of a palindrome to be detected</param>
        /// <param name="includeSpaces">Determines whether the space characters are treated like other characters or punctuations.</param>
        public PalindromeChecker(int minLength, int maxLength, bool includeSpaces)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
            this.includeSpaces = includeSpaces;
        }

        #endregion

        #region methods

        /// <summary>
        /// Clears the 'listOfPalindrome' list.
        /// </summary>
        public void ClearList()
        {
            listOfPalindromes.Clear();
        }

        /// <summary>
        /// Detects all palindromes in a string by finding all palindromes in a starting substring and 
        /// calling this same method on the rest of the string (proceeding one character at a time).
        /// </summary>
        /// <param name="text">A string to be examined.</param>
        /// <returns></returns>
        public List<string> FindAll(string text)
        {
            if (text.Length < minLength)
                return new List<string>();
            else
            {
                int max = maxLength;
                if (text.Length < maxLength)
                    max = text.Length;
                string subText = text.Substring(0, max);
                for (int i = max; i >= minLength; i--)
                {
                    if (!ObservedIn(subText.Substring(0, i), listOfPalindromes))
                    {
                        string subSubText = NoPunctuation(subText.Substring(0, i));
                        if (IsPalindrome(subSubText))
                        {
                            listOfPalindromes.Add(subText.Substring(0, i));
                            break;
                            
                        }
                    }
                }
                FindAll(text.Substring(1));
                return Refine(listOfPalindromes);
            }
        }

        /// <summary>
        /// Returns true if the string is a palindrome.
        /// </summary>
        /// <param name="input">The string to be examined.</param>
        /// <returns></returns>
        private bool IsPalindrome(string input)
        {
            return (input == Reverse(input));
        }

        /// <summary>
        /// Inverts the order of characters in a string.
        /// This is the basic manual reversal method.
        /// There are better faster methods available.
        /// </summary>
        /// <param name="input">The string to be reversed.</param>
        /// <returns></returns>
        private string Reverse_0(string input)
        {
            string output = "";
            for (int i = input.Length; i > 0; i--)
                output += input[i - 1];
            return output.ToString();
        }

        /// <summary>
        /// Inverts the order of characters in a string.
        /// This is a more efficient manual reversal method.
        /// This method has the highest efficiency for the current program.
        /// </summary>
        /// <param name="input">The string to be reversed.</param>
        /// <returns></returns>
        private string Reverse(string input)
        {
            StringBuilder output = new StringBuilder();
            for (int i = input.Length; i > 0; i--)
                output.Append(input[i - 1]);
            return output.ToString();
        }

        /// <summary>
        /// Inverts the order of characters in a string.
        /// This is a reversal method using Array.Reverse() method.
        /// In general, this method is more efficient for longer string (outside the scope of this program).
        /// </summary>
        /// <param name="input">The string to be reversed.</param>
        /// <returns></returns>
        private string Reverse_2(string input)
        {
            return new string(input.ToCharArray().Reverse().ToArray());
        }

        /// <summary>
        /// Inverts the order of characters in a string.
        /// This reversal method uses XOR on a character array.
        /// Note: This method doesn't support Unicode-32bit.
        /// </summary>
        /// <param name="input">The string to be reversed.</param>
        /// <returns></returns>
        private string Reverse_3(string input)
        {
            char[] charArray = input.ToCharArray();
            int len = input.Length - 1;
            for (int i = 0; i < len; i++, len--)
            {
                charArray[i] ^= charArray[len];
                charArray[len] ^= charArray[i];
                charArray[i] ^= charArray[len];
            }
            return new string(charArray);
        }

        /// <summary>
        /// Removes all punctuations from a given string.
        /// </summary>
        /// <param name="input">A string to be punctuation free.</param>
        /// <returns></returns>
        private string NoPunctuation(string input)
        {
            //string[] punctuations = {" ",".", ",", "،", "、","'","\"","‘","’","“","”",":",";","!","?","‹","›","«","»",
            //    "‐","_","‒","–","—","―","\\","/","⁄","|","#","~","@"};
            foreach (string p in punctuations)
            {
                input = input.Replace(p, "");
            }
            return input;
        }

        /// <summary>
        /// Determines whether a string is part of any item in a list of strings.
        /// </summary>
        /// <param name="myString">A string to be examined</param>
        /// <param name="list">A list of string items to check for partial matches.</param>
        /// <returns></returns>
        private bool ObservedIn(string myString, List<string> list)
        {
            foreach (string listedString in list)
            {
                if (listedString.Contains(myString))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Refines a list of strings by calling Refine() method on each item 
        /// followed by examining the length threshold and space/no space option.
        /// In case of including spaces, the refined string must be a palindrome.
        /// </summary>
        /// <param name="input">A list of strings to be refined.</param>
        /// <returns></returns>
        private List<string> Refine(List<string> input)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < input.Count; i++)
            {
                string refined = Refine(input[i]);
                if (refined.Length >= minLength &&
                    (!includeSpaces || (includeSpaces && IsPalindrome(refined))))
                    output.Add(refined);
            }
            return output;
        }

        /// <summary>
        /// Refines a string by 1)removing all punctuations from both sides of it, 
        /// 2)replacing every sequence of punctuation in the middle with a single space character, 
        /// in order to preserve the readability of the results.
        /// </summary>
        /// <param name="input">A string to be refined.</param>
        /// <returns></returns>
        private string Refine(string input)
        {
            string output = "";
            bool nonPunctuationObserved = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (punctuations.Contains(input[i].ToString()))
                {
                    if (nonPunctuationObserved && output[output.Length - 1] != ' ')
                            output += " ";
                }
                else
                {
                    nonPunctuationObserved = true;
                    output += input[i];
                }
            }
            if (output[output.Length - 1] == ' ')
                return output.Substring(0, output.Length - 1);
            else
                return output;
        }

        #endregion

    }
}
