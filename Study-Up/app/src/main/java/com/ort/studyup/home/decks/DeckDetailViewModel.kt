package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.Flashcard
import com.ort.studyup.common.models.User
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.UserRepository

class DeckDetailViewModel(
    private val deckRepository: DeckRepository,
    private val userRepository: UserRepository
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


}