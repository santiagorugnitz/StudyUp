package com.ort.studyup.services

class ServiceError(
    var code:Int,
    var message:String,
){
    override fun toString(): String {
        return message
    }
}