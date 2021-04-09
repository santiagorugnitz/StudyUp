package com.ort.studyup.home

import android.os.Bundle
import androidx.lifecycle.LiveData
import androidx.navigation.NavController
import com.ort.studyup.R
import com.ort.studyup.common.setupWithNavController
import com.ort.studyup.common.ui.BaseActivity
import kotlinx.android.synthetic.main.activity_home.*

class HomeActivity : BaseActivity() {

    private var currentNavController: LiveData<NavController>? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_home)
        if (savedInstanceState == null) setUpBottomNavigationBar()

    }

    override fun onRestoreInstanceState(savedInstanceState: Bundle) {
        super.onRestoreInstanceState(savedInstanceState)
        setUpBottomNavigationBar()
    }

    private fun setUpBottomNavigationBar() {
        val navGraphsIds = listOf(R.navigation.decks_nav)

        val controller = bottomBar.setupWithNavController(
            navGraphsIds,
            supportFragmentManager,
            R.id.nav_host_container,
            intent
        )
        currentNavController = controller
    }

    override fun onSupportNavigateUp(): Boolean = currentNavController?.value?.navigateUp() ?: false

}