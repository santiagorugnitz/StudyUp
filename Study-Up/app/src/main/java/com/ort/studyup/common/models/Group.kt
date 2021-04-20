package com.ort.studyup.common.models

import com.google.gson.annotations.SerializedName


class Group(
        name: String,
)

class NewGroupRequest(
        @SerializedName("name") val name: String,
)

class GroupSearchResponse(
        @SerializedName("id") val id: Int,
        @SerializedName("name") val name: String,
        @SerializedName("subscribed") val subscribed: Boolean,
        @SerializedName("teacherName") val teacherName: String,
)
