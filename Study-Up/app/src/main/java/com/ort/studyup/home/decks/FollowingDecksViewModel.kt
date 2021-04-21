package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.DeckRepository

class FollowingDecksViewModel(
    private val deckRepository: DeckRepository,
    private val resourceWrapper: ResourceWrapper
) : BaseViewModel() {

    private val items = mutableListOf<Any>()
    private var decks = mutableListOf<Deck>()
    var authors = arrayOf<String>()

    fun loadDecks(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            decks = deckRepository.getFollowingDecks().sortedWith { a, b -> a.subject.compareTo(b.subject) } as MutableList<Deck>
            val authorList = mutableListOf(resourceWrapper.getString(R.string.everyone))
            authorList.addAll(decks.map { it.creator }.distinct())
            authors = authorList.toTypedArray()
            result.postValue(items)
        }
        return result
    }

    private fun prepareDisplayList(decks: List<Deck>) {
        items.clear()
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