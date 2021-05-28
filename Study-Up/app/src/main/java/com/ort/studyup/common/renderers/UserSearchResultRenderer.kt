package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_user_search_result.view.*

class UserSearchResultRenderer(private val callback: Callback) : ViewRenderer<UserSearchResultRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_user_search_result, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.username.text = model.username
        view.followButton.setOnClickListener {
            callback.onFollowChange(position)
        }
        if (model.following) {
            view.followButton.text = view.context.getString(R.string.unfollow)
            view.followButton.setBackgroundColor(view.resources.getColor(R.color.error,null))
        } else {
            view.followButton.text = view.context.getString(R.string.follow)
            view.followButton.setBackgroundColor(view.resources.getColor(R.color.green,null))
        }
    }

    class Item(
        val username: String,
        var following: Boolean
    )

    interface Callback {
        fun onFollowChange(position:Int)
    }
}