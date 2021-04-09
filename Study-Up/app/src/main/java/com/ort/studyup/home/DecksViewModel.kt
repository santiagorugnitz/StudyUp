package com.ort.studyup.home

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.UserRepository

class DecksViewModel(
//    val userRepository: UserRepository,
//    val deckRepository: DeckRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadDecks(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            //val user = userRepository.getUser()
           // user?.let{
                //var decks = deckRepository.decksFromUser(user.id)
                //decks = decks.sortedWith { a, b -> a.subject.compareTo(b.subject) }
                //TODO: remove hardcoded values
                val decks = listOf(
                    Deck(0,"test1",0,"Subject1",null,false),
                    Deck(0,"test2",0,"Subject1",null,false),
                    Deck(0,"test3",0,"Subject1",null,false),
                    Deck(0,"test4",0,"Subject2",null,false),
                    Deck(0,"test5",0,"Subject2",null,false),
                    )

                var currentSubject = ""
                decks.forEach {
                    if(it.subject!=currentSubject){
                        currentSubject=it.subject
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
           // }
        }
        return result

    }
}