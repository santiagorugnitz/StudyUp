package com.ort.studyup.common.ui

import android.content.Context
import android.util.AttributeSet
import android.view.View
import android.widget.FrameLayout
import androidx.annotation.DrawableRes
import com.ort.studyup.R
import com.ort.studyup.common.getActivity
import kotlinx.android.synthetic.main.confirmation_dialog.view.*

class ConfirmationDialog(
    context: Context,
    private val message: String,
    private val callback: Callback,
    @DrawableRes private val icon: Int = R.drawable.ic_warning,
    private val buttonText: String? = null,
    attrs: AttributeSet? = null
) : View(context, attrs) {


    fun show() {
        val view = inflate()
        view.dialogMsg.text = message
        view.dialogIcon.setImageResource(icon)
        buttonText?.let {
            view.dialogButton.text = it
        }

        view.dialogButton.setOnClickListener { callback.onButtonClick() }
        view.closeIcon.setOnClickListener { hide() }
    }


    private fun inflate(): View {
        val view = getActivity()?.findViewById<View>(R.id.confirmationDialogContainer)
        if (view != null) return view
        val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
        return inflate(context, R.layout.confirmation_dialog, root)
    }

    fun hide() {
        try {
            val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
            val view = root?.findViewById<FrameLayout>(R.id.confirmationDialogContainer)
            root?.removeView(view)
        } catch (e: Exception) {
            e
        }
    }

    interface Callback {
        fun onButtonClick()
    }

}