// Jenkinsfile template for Unity 2017.4.27f1 (no Build Profiles).
// Builds the scenes enabled in File > Build Settings for each TARGET with
// Assets/Editor/CommandLineBuild.cs. See "Configuring a Legacy Unity Build Pipeline.docx".
//
// Unity 2017.4 notes:
// - Standalone players (Linux64, Win64) build with Mono only. IL2CPP is not available
//   for standalone targets in this version; the editor script switches to Mono if needed.
// - BuildPipeline.BuildPlayer returns a string here, not a BuildReport. The editor
//   script handles this; harmless errors logged under -nographics do not fail the build.
// - Agent image modules (UNITY_INSTALLS): 2017.4.27f1:0c4b856e4c6e:windows,webgl
def buildDir(String target, String buildType) {
  def projectPrefix = (env.PROJECT_DIR == '.') ? '' : "${env.PROJECT_DIR}/"
  return "${projectPrefix}Builds/${target}/${buildType}"
}

pipeline {
  agent none
  environment {
    // Folder of the Unity project (the one with Assets and ProjectSettings),
    // relative to the repository root. '.' when the project is at the root.
    PROJECT_DIR = '.'
  }
  options {
    // Declarative otherwise checks out on every stage that allocates an agent,
    // which would sync the project twice in each build cell. Checkout is done
    // explicitly in the Checkout stage instead.
    skipDefaultCheckout()

    // Keep archived artifacts only for the most recent build (Jenkins also always
    // keeps the last successful build's artifacts), but keep the build records,
    // including their console logs with the full Unity output, for the last 25 builds.
    buildDiscarder(logRotator(numToKeepStr: '25', artifactNumToKeepStr: '1'))
  }
  stages {
    stage("Build and test") {
      failFast true
      matrix {
        axes {
          axis {
            // Unity -buildTarget names. Remove the platforms this project doesn't ship.
            name 'TARGET'
            values 'Linux64', 'Win64', 'WebGL'
          }
          axis {
            // Add 'Development' to also build development players
            name 'BUILD_TYPE'
            values 'Release'
          }
          axis {
            name 'OS'
            values 'linux'
          }
        }
        // No agent at the matrix level: a matrix cell doesn't claim an
        // executor until the stage below has acquired the license lock.
        stages {
          stage("Licensed build") {
            options {
              // Stage-level options are applied BEFORE the stage's agent is
              // allocated, so cells queue for the lock on the controller
              // without holding an executor.
              lock('unity-license')
            }
            agent {
              label "${OS}"
            }
            stages {
              stage("Checkout") {
                steps {
                  checkout scm
                }
              }
              stage("Build") {
                steps {
                  // Start from an empty output folder so files from earlier
                  // builds can't end up archived.
                  sh "rm -f 'build-${TARGET}_${BUILD_TYPE}.log'"
                  sh "rm -rf '${buildDir(TARGET, BUILD_TYPE)}'"

                  withCredentials([
                    string(
                      credentialsId: 'unity_license_serial',
                      variable: 'UNITY_LICENSE_SERIAL'
                    ),
                    usernamePassword(
                      credentialsId: 'unity_license_user',
                      usernameVariable: 'UNITY_USER',
                      passwordVariable: 'UNITY_PASSWORD'
                    )
                  ]) {
                    // Output Unity logs to standard output (/dev/stdout) and pipe to file via tee.
                    // Runs under bash with pipefail so the step fails when Unity fails,
                    // instead of reporting tee's (always successful) exit code.
                    // -projectPath must be absolute: Unity 2017 rejects "-projectPath .".
                    sh '''#!/bin/bash
                      set -eo pipefail
                      DEV_FLAG=""
                      if [ "$BUILD_TYPE" = "Development" ]; then DEV_FLAG="-devBuild"; fi
                      xvfb-run --auto-servernum --server-args='-screen 0 1024x768x24' dbus-launch \
                      ~/Unity/Hub/Editor/2017.4.27f1/Editor/Unity \
                      -username "$UNITY_USER" \
                      -password "$UNITY_PASSWORD" \
                      -serial "$UNITY_LICENSE_SERIAL" \
                      -batchmode \
                      -nographics \
                      -quit \
                      -projectPath "$PWD/$PROJECT_DIR" \
                      $DEV_FLAG \
                      -buildTarget "$TARGET" \
                      -buildType "$BUILD_TYPE" \
                      -executeMethod CommandLineBuild.Build \
                      -logFile /dev/stdout | tee "build-${TARGET}_${BUILD_TYPE}.log"
                    '''
                  }
                }
                post {
                  always {
                    archiveArtifacts artifacts: "build-${TARGET}_${BUILD_TYPE}.log", allowEmptyArchive: true, fingerprint: true
                  }
                }
              }
              stage("Archive") {
                steps {
                  archiveArtifacts artifacts: "${buildDir(TARGET, BUILD_TYPE)}/**/*", allowEmptyArchive: false, fingerprint: true
                }
              }
            }
          }
        }
      }
    }
  }
}
