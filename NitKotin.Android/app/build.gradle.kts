import com.android.build.api.dsl.ManagedVirtualDevice

plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("org.jetbrains.kotlin.plugin.compose")
    id("org.jetbrains.kotlin.plugin.serialization")
}

android {
    namespace = "com.nitkotin.android"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.nitkotin.android"
        minSdk = 26
        targetSdk = 35
        versionCode = 1
        versionName = "0.1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
        vectorDrawables.useSupportLibrary = true
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }

    kotlinOptions {
        jvmTarget = "17"
    }

    buildFeatures {
        compose = true
        buildConfig = true
    }

    packaging {
        resources {
            excludes += "/META-INF/{AL2.0,LGPL2.1}"
        }
    }

    testOptions {
        managedDevices {
            localDevices {
                create("pixel6Api34") {
                    device = "Pixel 6"
                    apiLevel = 34
                    systemImageSource = "aosp-atd"
                }

                create("pixelTabletApi34") {
                    device = "Pixel Tablet"
                    apiLevel = 34
                    systemImageSource = "aosp-atd"
                }
            }

            groups {
                create("smoke") {
                    targetDevices.add(devices["pixel6Api34"])
                }

                create("formFactors") {
                    targetDevices.add(devices["pixel6Api34"])
                    targetDevices.add(devices["pixelTabletApi34"])
                }
            }
        }
    }
}

tasks.register("ciAndroidLocal") {
    group = "verification"
    description = "Builds the app, unit tests, and androidTest APKs without requiring a running device."
    dependsOn(
        ":app:assembleDebug",
        ":app:testDebugUnitTest",
        ":app:assembleDebugAndroidTest",
    )
}

tasks.register("ciAndroidManagedSmoke") {
    group = "verification"
    description = "Runs Android instrumentation smoke tests on the managed Pixel 6 API 34 device."
    dependsOn(":app:pixel6Api34DebugAndroidTest")
}

tasks.register("ciAndroidManagedFormFactors") {
    group = "verification"
    description = "Runs Android instrumentation tests on the managed phone and tablet devices."
    dependsOn(":app:formFactorsGroupDebugAndroidTest")
}

dependencies {
    val composeBom = platform("androidx.compose:compose-bom:2024.09.03")

    implementation("androidx.core:core-ktx:1.15.0")
    implementation("androidx.core:core-splashscreen:1.0.1")
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.8.7")
    implementation("androidx.lifecycle:lifecycle-runtime-compose:2.8.7")
    implementation("androidx.lifecycle:lifecycle-viewmodel-compose:2.8.7")
    implementation("androidx.activity:activity-compose:1.10.1")
    implementation("androidx.datastore:datastore-preferences:1.1.1")
    implementation("androidx.work:work-runtime-ktx:2.10.0")
    implementation("com.google.android.material:material:1.12.0")
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.9.0")
    implementation("org.jetbrains.kotlinx:kotlinx-serialization-json:1.7.3")

    implementation(composeBom)
    androidTestImplementation(composeBom)
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    implementation("androidx.compose.material3:material3-window-size-class")
    implementation("androidx.compose.material:material-icons-extended")
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.2.1")
    androidTestImplementation("androidx.test:core-ktx:1.6.1")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.6.1")
    androidTestImplementation("androidx.compose.ui:ui-test-junit4")
    debugImplementation("androidx.compose.ui:ui-tooling")
    debugImplementation("androidx.compose.ui:ui-test-manifest")
}
