package com.ort.studyup.home.groups

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.GroupRepository
import com.ort.studyup.services.ServiceError

class NewGroupViewModel(
    private val resourceWrapper: ResourceWrapper,
    private val groupRepository: GroupRepository
) : BaseViewModel() {


    fun createGroup(name: String): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            if (name.isNotEmpty()) {
                    groupRepository.createGroup(name)
                result.postValue(true)
            } else {
                error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.error_empty_fields)))
                result.postValue(false)
            }
        }
        return result
    }

}