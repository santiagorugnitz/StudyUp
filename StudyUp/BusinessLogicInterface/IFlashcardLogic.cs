﻿using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IFlashcardLogic
    {
        Flashcard AddFlashcard(Flashcard flashcard, int userId);
        Flashcard EditFlashcard(string token, int flashcardId, string newQuestion, string newAnswer);
    }
}
