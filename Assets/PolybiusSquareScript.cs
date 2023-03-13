using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;
using KModkit;

public class PolybiusSquareScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;
    public TextMesh[] Display;
    public TextMesh[] ButtonLabels;

    public KMSelectable[] Buttons;

    int[] numbers = {-1, -1, -1, -1, -1};

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private string _solution;
    private string _answerSoFar;
    private bool _moduleSolved;

    private void Start()
    {
        _moduleId = _moduleIdCounter++;

        // Generate Display digits

        for (int i = 0; i < 5; i++)
        {
            numbers[i] = (Rnd.Range(1, 6) * 10) + Rnd.Range(1, 6);
            Display[i].text = numbers[i].ToString();
        }

        Debug.LogFormat("[Polybius Square #{0}] The display is {1}", _moduleId, (Display[0].text + " " + Display[1].text + " " + Display[2].text + " " + Display[3].text + " " + Display[4].text));

        //Run calculateSolution method.

        calculateSolution();


        List<int> chungus = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            int MargaretThatcher = Rnd.Range(0, 12);
            while (chungus.Contains(MargaretThatcher))
            {
                MargaretThatcher = Rnd.Range(0, 12);
            }
            chungus.Add(MargaretThatcher);
        }
        int counter = 0;
        bool big = false;
        if (Rnd.Range(0, 2) == 0)
        {
            big = true;
        }

        var pool = Enumerable.Range('A', 26).Select(i => (char)i).ToList();

        if (big)
        {
            pool.Remove('I');
            _solution = _solution.Replace('I', 'J');
        }
        else
        {
            pool.Remove('J');
        }

        List<char> GusFring = new List<char>();

        Debug.LogFormat("[Polybius Square #{0}] The solution is {1}", _moduleId, _solution);
        for (int i = 0; i < 12; i++)
        {

            var ix = Rnd.Range(0, pool.Count);
            while (_solution.Contains(pool[ix]))
            {
                ix = Rnd.Range(0, pool.Count);
            }
            var letter = pool[ix];
            var j = i;

            if (chungus.Contains(j))
            {
                if (!GusFring.Contains(_solution[counter]))
                {
                letter = _solution[counter];
                ix = pool.IndexOf(letter);
                    GusFring.Add(letter);
                }

                counter++;
            }

            ButtonLabels[i].text = letter.ToString();
            Buttons[i].OnInteract += delegate
            {
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Buttons[j].transform);
                Buttons[j].AddInteractionPunch();
                if (_moduleSolved)
                {
                    return false;
                }
                _answerSoFar += letter;
                Debug.LogFormat("[Polybius Square #{0}] You pressed {1}; answer now {2}", _moduleId, letter, _answerSoFar);
                if (_answerSoFar.Length == _solution.Length)
                {
                    if (_answerSoFar == _solution)
                    {
                        Debug.LogFormat("[Polybius Square #{0}] Module solved.", _moduleId);
                        Module.HandlePass();
                        _moduleSolved = true;
                    }
                    else
                    {
                        Debug.LogFormat("[Polybius Square #{0}] Wrong answer.", _moduleId);
                        Module.HandleStrike();
                    }
                    _answerSoFar = "";
                }
                return false;
            };
            pool.RemoveAt(ix);

        }


        




    }

    void calculateSolution()
    {

        int tableNumber = numbers[0] + numbers[1] + numbers[2] + numbers[3] + numbers[4];
        foreach (char letter in BombInfo.GetSerialNumber())
        {
            if ("0123456789".Contains(letter))
            {
                tableNumber += "0123456789".IndexOf(letter);
            }
            else if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(letter))
            {
                tableNumber += "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter) + 1;
            }
        }

        tableNumber = (tableNumber * BombInfo.GetBatteryCount()) % 10;
        string[][] Table = Tables(tableNumber);

        numbers[0] = numbers[0] - 11;
        numbers[1] = numbers[1] - 11;
        numbers[2] = numbers[2] - 11;
        numbers[3] = numbers[3] - 11;
        numbers[4] = numbers[4] - 11;


        _solution += Table[numbers[0] / 10][numbers[0] % 10];
        _solution += Table[numbers[1] / 10][numbers[1] % 10];
        _solution += Table[numbers[2] / 10][numbers[2] % 10];
        _solution += Table[numbers[3] / 10][numbers[3] % 10];
        _solution += Table[numbers[4] / 10][numbers[4] % 10];



    }
    string[][] Tables(int solution)
    {

        switch (solution) 
        {
            case 0:
            return new string[][]
        {

            new string[] {"A", "B", "C", "D", "E"},
            new string[] {"F", "G", "H", "I", "K"},
            new string[] {"L", "M", "N", "O", "P"},
            new string[] {"Q", "R", "S", "T", "U"},
            new string[] {"V", "W", "X", "Y", "Z"}
        };
            case 1:
                return new string[][]
            {
            new string[] {"F", "K", "I", "G", "H"},
            new string[] {"Q", "U", "T", "R", "S"},
            new string[] {"V", "Z", "Y", "W", "X"},
            new string[] {"A", "E", "D", "B", "C"},
            new string[] {"L", "P", "O", "M", "N"}

            };

            case 2:
                return new string[][]
            {
            new string[] {"C", "V", "B", "N", "M"},
            new string[] {"H", "K", "L", "Z", "X"},
            new string[] {"S", "I", "D", "F", "G"},
            new string[] {"Y", "U", "O", "P", "A"},
            new string[] {"Q", "W", "E", "R", "T"}
            };


            case 3:
                return new string[][]
           {
           new string[] {"Z", "X", "I", "Y", "W"},
           new string[] {"V", "S", "O", "T", "P"},
           new string[] {"L", "G", "C", "H", "D"},
           new string[] {"U", "R", "N", "Q", "M"},
           new string[] {"K", "F", "B", "E", "A"}

           };

            case 4:
                return new string[][]
            {
           new string[] {"E", "C", "A", "D", "B"},
           new string[] {"K", "H", "F", "I", "G"},
           new string[] {"P", "N", "L", "O", "M"},
           new string[] {"Z", "X", "V", "Y", "W"},
           new string[] {"U", "S", "Q", "T", "R"}

            };

            case 5:
                return new string[][]
            {
           new string[] {"A", "R", "L", "W", "Z"},
           new string[] {"F", "M", "B", "S", "X"},
           new string[] {"I", "C", "G", "N", "T"},
           new string[] {"U", "H", "P", "D", "O"},
           new string[] {"Y", "Q", "V", "K", "E"}

            };
            case 6:
                return new string[][]
            {
           new string[] {"X", "G", "K", "N", "H"},
           new string[] {"T", "F", "O", "Y", "U"},
           new string[] {"C", "V", "B", "I", "A"},
           new string[] {"L", "M", "Q", "Z", "P"},
           new string[] {"D", "S", "R", "E", "W"}

            };
            case 7:
                return new string[][]
            {
           new string[] {"A", "S", "F", "K", "H"},
           new string[] {"Z", "E", "D", "N", "G"},
           new string[] {"W", "M", "C", "Q", "I"},
           new string[] {"Y", "P", "X", "R", "O"},
           new string[] {"T", "V", "B", "U", "L"}

            };

            case 8:
                return new string[][]
            {
           new string[] {"Z", "Y", "X", "W", "V"},
           new string[] {"U", "T", "S", "R", "Q"},
           new string[] {"P", "O", "N", "M", "L"},
           new string[] {"K", "I", "H", "G", "F"},
           new string[] {"E", "D", "C", "B", "A"}

            };
            default:
                return new string[][]
            {
           new string[] {"E", "D", "C", "B", "A"},
           new string[] {"K", "I", "H", "G", "F"},
           new string[] {"P", "O", "N", "M", "L"},
           new string[] {"U", "T", "S", "R", "Q"},
           new string[] {"Z", "Y", "X", "W", "V"}

            };

        }
    }
#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} press ABCDE [Press buttons with labels A, B, C, D, E.] | Command must contain 5 letters.";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        var m = Regex.Match(command, @"^\s*(press|submit)\s+(?<letters>[A-Z ,;]+)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!m.Success)
            yield break;
        _answerSoFar = "";
        var input = m.Groups["letters"].Value.ToUpperInvariant();
        var btnLetters = ButtonLabels.Select(i => i.text).ToArray();
        var list = new List<int>();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].ToString() == " " || input[i].ToString() == "," || input[i].ToString() == ";")
                continue;
            int index = Array.IndexOf(btnLetters, input[i].ToString());
            if (index == -1)
            {
                yield return "sendtochaterror The letter " + input[i].ToString() + " does not exist!";
                yield break;
            }
            list.Add(index);
        }
        if (list.Count != 5)
            yield break;
        yield return null;
        for (int i = 0; i < 5; i++)
        {
            Buttons[list[i]].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        _answerSoFar = "";
        var btnLetters = ButtonLabels.Select(i => i.text).ToArray();
        for (int i = 0; i < 5; i++)
        {
            Buttons[Array.IndexOf(btnLetters, _solution[i].ToString())].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }
}

