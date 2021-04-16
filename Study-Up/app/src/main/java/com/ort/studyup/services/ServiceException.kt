package com.ort.studyup.services

import retrofit2.Response

class ServiceException(response: Response<*>) : Exception() {

    var error: ServiceError = ServiceError(response.code(),response.message())

}
