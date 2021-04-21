package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.UserRepository

class DecksViewModel(
    private val userRepository: UserRepository,
    private val deckRepository: DeckRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadDecks(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            items.clear()
            val user = userRepository.getUser()
            user?.let {
                var decks = deckRepository.decksFromUser(user.id)
                decks = decks.sortedWith(Comparator { a, b -> a.subject.compareTo(b.subject) })

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
                result.postValue(items)
            }
        }
        return result
    }

}