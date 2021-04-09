package com.ort.studyup.services

import retrofit2.Response

class ServiceException(response: Response<*>) : Exception() {

    lateinit var error: ServiceError

    init {
        //TODO: use parser to get message from body instead of directly using response message
        this.error.code = response.code()
        this.error.message = response.message()
    }

}
