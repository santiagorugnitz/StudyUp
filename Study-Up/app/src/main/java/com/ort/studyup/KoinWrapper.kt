package com.ort.studyup

import android.app.Application
import androidx.room.Room
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.common.utils.EncryptedPreferencesHelper
import com.ort.studyup.login.LoginViewModel
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceFactory
import com.ort.studyup.services.UserService
import com.ort.studyup.splash.SplashViewModel
import com.ort.studyup.storage.dao.AppDatabase
import org.koin.android.ext.koin.androidContext
import org.koin.android.ext.koin.androidLogger
import org.koin.core.context.startKoin
import org.koin.dsl.module

object KoinWrapper {

    fun start(application: Application) {
        startKoin {
            androidLogger()
            androidContext(application)
            modules(
                listOf(
                    utils(),
                    viewModels(),
                    repositories(),
                    //services(),
                    database(),
                )
            )
        }
    }

    private fun utils() = module {
        factory { ResourceWrapper(get()) }
        factory { EncryptedPreferencesHelper(get()) }

    }

    private fun viewModels() = module {
        factory { LoginViewModel(get()) }
        factory { SplashViewModel() }
    }

    private fun repositories() = module {
        factory { UserRepository(get(), get(),get()) }
    }

    private fun services() = module {
        factory { ServiceFactory().createInstance(UserService::class.java) }
    }

    private fun database() = module {
        single {
            Room.databaseBuilder(
                androidContext(),
                AppDatabase::class.java, "studyUpDatabase"
            )
                .fallbackToDestructiveMigration()
                .build()
        }
        factory { get<AppDatabase>().userDao() }
    }

}