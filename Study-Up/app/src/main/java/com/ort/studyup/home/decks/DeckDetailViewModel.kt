package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.FlashcardRepository
import com.ort.studyup.repositories.UserRepository

class DeckDetailViewModel(
    private val deckRepository: DeckRepository,
    private val userRepository: UserRepository,
    private val flashcardRepository: FlashcardRepository
) : BaseViewModel() {

    var isOwner: Boolean = false

    fun loadDetails(id: Int): LiveData<Deck> {
        val result = MutableLiveData<Deck>()
        executeService {
            val deck = deckRepository.getDeck(id)
            isOwner = userRepository.getUser()?.username == deck.author
            result.postValue(deck)
        }
        return result
    }

    fun deleteComment(flashcardId: Int, commentId: Int): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            flashcardRepository.deleteComment(flashcardId, commentId)
            result.postValue(true)
        }
        return result
    }


}