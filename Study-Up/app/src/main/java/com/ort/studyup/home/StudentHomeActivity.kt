package com.ort.studyup.home

import android.os.Bundle
import android.view.MenuItem
import androidx.lifecycle.LiveData
import androidx.navigation.NavController
import com.ort.studyup.R
import com.ort.studyup.common.setupWithNavController
import com.ort.studyup.common.ui.BaseActivity
import kotlinx.android.synthetic.main.activity_student_home.*

class StudentHomeActivity : BaseActivity() {

    private var currentNavController: LiveData<NavController>? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_student_home)
        if (savedInstanceState == null) setUpBottomNavigationBar()

    }

    override fun onRestoreInstanceState(savedInstanceState: Bundle) {
        super.onRestoreInstanceState(savedInstanceState)
        setUpBottomNavigationBar()
    }

    private fun setUpBottomNavigationBar() {
        val navGraphsIds =
            listOf(
                R.navigation.tasks_nav,
                R.navigation.decks_nav,
                R.navigation.search_nav,
                R.navigation.following_nav,
                R.navigation.profile_nav,
            )

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