package com.ort.studyup.common.ui

import android.content.Context
import android.util.AttributeSet
import android.view.View
import android.widget.FrameLayout
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.core.content.ContextCompat
import com.ort.studyup.R
import com.ort.studyup.common.getActivity

class CustomLoader(context: Context, attrs: AttributeSet?) : View(context, attrs) {

    class LoaderConfig(val show: Boolean, val visibility: Visibility = Visibility.TRANSLUCENT)

    enum class Visibility { TRANSLUCENT, TRANSPARENT, NONE }

    fun show(visibility: Visibility) {
        when (visibility) {
            Visibility.TRANSLUCENT -> showTranslucent()
            Visibility.TRANSPARENT -> showTransparent()
            else -> return
        }
    }

    private fun showTranslucent(){
        val view = inflate()
    }

    private fun showTransparent() {
        val view = inflate()
        view.findViewById<ConstraintLayout>(R.id.loaderContainer)
            .setBackgroundColor(ContextCompat.getColor(context, android.R.color.transparent))
    }

    private fun inflate(): View {
        val view = getActivity()?.findViewById<View>(R.id.loaderContainer)
        if (view != null) return view
        val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
        return inflate(context, R.layout.custom_loader, root)
    }

    fun hide() {
        try {
            val root = getActivity()?.findViewById<FrameLayout>(android.R.id.content)
            val view = root?.findViewById<ConstraintLayout>(R.id.loaderContainer)
            root?.removeView(view)
        } catch (e: Exception) {
            e
        }
    }

}