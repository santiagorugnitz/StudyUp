using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IFlashcardLogic
    {
        Flashcard AddFlashcard(Flashcard flashcard, int deckId, string token);
        Flashcard EditFlashcard(string token, int flashcardId, string newQuestion, string newAnswer);
        bool DeleteFlashcard(int id, string token);
        Flashcard UpdateScore(int id, int score, string token);
        List<Tuple<Flashcard, int>> GetRatedFlashcards(int deckId, string token);
        void CommentFlashcard(int id, string token, string comment);
        bool DeleteComment(string token, int flashcardId, int commentId);
    }
}
