using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chattprogram
{
    class Element
    {
        int integer;
        char letter;
        List<char> alphabet = new List<char> { 'A' , 'B' , 'C' , 'D' , 'E', 'F', 'G' , 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
        bool sorted;

        public Element(int integer, char letter)
        {
            this.integer = integer;
            this.letter = letter;
        }

        public void GenerateElements(List<Element> elements)
        {
            for (int i = 0; i < 26; i++)
            {
                Element element = new Element(i, alphabet[i]);
                elements.Add(element);
            }
        }

        public void SortProfiles(List<Element> elements, List<Profile> profiles)
        {
            for (int i = 0; i < 26; i++)
            { 
                for (int j = 0; j < profiles.Count; j++)
                {
                    if (elements[i].letter == profiles[j].name[0])
                    {
                        profiles[j].value = elements[i].integer;
                    }
                }
            }

            while (sorted != true)
            {
                sorted = true;

                for (int i = 0; i < profiles.Count - 1; i++)
                {
                    if (profiles[i + 1].value < profiles[i].value)
                    {
                        Profile temp = profiles[i];

                        profiles[i] = profiles[i + 1];

                        profiles[i + 1] = temp;

                        sorted = false;
                    }
                }
            }
        }
    }
}
