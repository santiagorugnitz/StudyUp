package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.Flashcard
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseViewModel

class DeckDetailViewModel(
//    val deckRepository: DeckRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadDetails(id: Int): LiveData<Deck> {
        val result = MutableLiveData<Deck>()
        executeService {
            items.clear()
            //val deck = deckRepository.getDeck(id)
            //TODO: remove hardcoded values
            val deck = Deck(1,
                    "Santiago",
                    "Design Patterns",
                    2,
                    "Software Architecture",
                    false,
                    listOf(
                            Flashcard(1, "2+2?", "4"),
                            Flashcard(1, "0==0?", "true"),
                            Flashcard(1, "Which pattern would you use when having a system with multiple stuff", "yes"),
                    ))
            result.postValue(deck)
        }
        return result
    }

}