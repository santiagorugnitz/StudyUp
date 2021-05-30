using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogicInterface
{
    public interface IFlashcardLogic
    {
        Flashcard AddFlashcard(Flashcard flashcard, int deckId, string token);
        void CommentFlashcard(int id, string token, string comment);
        bool DeleteComment(string token, int flashcardId, int commentId);
        bool DeleteFlashcard(int id, string token);
        Flashcard EditFlashcard(string token, int flashcardId, Flashcard editedFlashcard);
        List<Tuple<Flashcard, int>> GetRatedFlashcards(int deckId, string token);
        Flashcard UpdateScore(int id, int score, string token);
    }
}
