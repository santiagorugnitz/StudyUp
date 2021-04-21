package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.UserRepository

class FollowingDecksViewModel(
    private val deckRepository: DeckRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()
    private var decks = mutableListOf<Deck>()
    var authors = arrayOf<String>()

    fun loadDecks(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            items.clear()
            decks = deckRepository.getFollowingDecks().sortedWith { a, b -> a.subject.compareTo(b.subject) } as MutableList<Deck>
            prepareDisplayList(decks)
            authors = decks.map { it.creator }.distinct().toTypedArray()
            result.postValue(items)
        }
        return result
    }

    private fun prepareDisplayList(decks: List<Deck>) {
        var currentSubject = ""
        decks.forEach {
            if (it.subject != currentSubject) {
                currentSubject = it.subject
                items.add(
                    SubtitleRenderer.Item(currentSubject)
                )
            }
            items.add(
                DeckItemRenderer.Item(
                    it.id,
                    it.name
                )
            )
        }
    }

    fun filterDecks(author: String?): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            author?.let {
                prepareDisplayList(decks.filter { it.creator == author })
            } ?: run {
                prepareDisplayList(decks)
            }
            result.postValue(items)
        }

        return result
    }


}