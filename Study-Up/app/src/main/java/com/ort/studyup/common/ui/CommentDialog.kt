package com.ort.studyup.common.ui

import android.content.Context
import android.text.InputType
import android.util.AttributeSet
import android.view.View
import android.widget.FrameLayout
import androidx.core.widget.doOnTextChanged
import com.ort.studyup.R
import com.ort.studyup.common.getActivity
import kotlinx.android.synthetic.main.comment_dialog.view.*
import kotlinx.android.synthetic.main.confirmation_dialog.view.*
import kotlinx.android.synthetic.main.confirmation_dialog.view.closeIcon
import kotlinx.android.synthetic.main.confirmation_dialog.view.dialogButton

class CommentDialog(
    context: Context,
    private val callback: Callback,
    attrs: AttributeSet? = null
) : View(context, attrs) {


    fun show() {
        val view = inflate()
        view.dialogButton.setOnClickListener {
            callback.onComment(view.commentInput.text.toString())
            hide()
        }
        view.closeIcon.setOnClickListener { hide() }
        view.dialogButton.isEnabled = false
        view.commentInput.doOnTextChanged { text, _, _, _ -> view.dialogButton.isEnabled = text?.length ?: 0 > 0 }
    }


    private fun inflate(): View {
        val view = getActivity()?.findViewById<View>(R.id.commentDialogContainer)
        if (view != null) return view
        val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
        return inflate(context, R.layout.comment_dialog, root)
    }

    fun hide() {
        try {
            val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
            val view = root?.findViewById<FrameLayout>(R.id.commentDialogContainer)
            root?.removeView(view)
        } catch (e: Exception) {
            e
        }
    }

    interface Callback {
        fun onComment(comment: String)
    }

}