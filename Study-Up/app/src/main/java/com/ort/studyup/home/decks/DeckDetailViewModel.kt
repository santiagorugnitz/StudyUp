package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.Flashcard
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository

class DeckDetailViewModel(
    private val deckRepository: DeckRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadDetails(id: Int): LiveData<Deck> {
        val result = MutableLiveData<Deck>()
        executeService {
            items.clear()
            val deck = deckRepository.getDeck(id)
            result.postValue(deck)
        }
        return result
    }

}