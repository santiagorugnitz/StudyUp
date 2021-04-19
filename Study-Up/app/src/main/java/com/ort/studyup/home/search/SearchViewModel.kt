package com.ort.studyup.home.search

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.User
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.GroupRepository
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class SearchViewModel(
) : BaseViewModel() {

    var searchUsers= true

}