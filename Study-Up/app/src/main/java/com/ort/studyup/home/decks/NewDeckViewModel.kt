package com.ort.studyup.home.decks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.services.ServiceError

class NewDeckViewModel(
    private val resourceWrapper: ResourceWrapper
//    val deckRepository: DeckRepository
) : BaseViewModel() {

    //TODO uncomment when ready
    var deckId = -1

    fun sendData(data: DeckData): LiveData<Int> {
        val result = MutableLiveData<Int>()
        executeService {
            if (validate(data)) {
                if (deckId != -1) {
                    //deckRepository.updateDeck(deckId,data)
                } else {
                    //deckId = deckRepository.createDeck(data).id
                }
                result.postValue(deckId)
            } else {
                error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.error_empty_fields)))
                result.postValue(-1)
            }
        }
        return result
    }

    private fun validate(data: DeckData): Boolean {
        return data.name.isNotEmpty() && data.subject.isNotEmpty()
    }

    fun deleteDeck():LiveData<Boolean>{
        val result = MutableLiveData<Boolean>()
        executeService {
            //deckRepository.delete(deckId)
            result.postValue(true)
        }
        return result
    }

}