using System;
using Perevodchik;

class Program
{
    static void Main( string[] args )
    {
        Translator.LoadDictionary();
        MenuManager.ShowMainMenu();
    }
}