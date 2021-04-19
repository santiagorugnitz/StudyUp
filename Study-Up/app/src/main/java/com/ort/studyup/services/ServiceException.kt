package com.ort.studyup.services

class ServiceException(code: Int, msg: String) : Exception() {

    var error: ServiceError = ServiceError(code, msg)

}
