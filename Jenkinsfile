pipeline {
    environment {
      PROJECT = "myproject-ahsan-123"
      APP_NAME = "aspnet-microservices-gke-crun"
      IMAGE_TAG = "gcr.io/${PROJECT}/${APP_NAME}"
    }
    agent any
    stages {
        stage('Project Activation') {
          steps {
            withCredentials([[$class: 'FileBinding', credentialsId:"gcloud", variable: 'JSON_KEY']]) {
              sh 'gcloud auth activate-service-account --key-file $JSON_KEY'
              sh 'gcloud --version'
            }
          }    
        }
        stage('Build and push image with Container Builder') {
          steps {
            sh 'gcloud builds submit -t ${IMAGE_TAG} .'
          }
        }
        stage('Create GKE cluster') {
          steps {
            sh 'gcloud container clusters create ${APP_NAME} --addons=HorizontalPodAutoscaling,HttpLoadBalancing,CloudRun --machine-type=n1-standard-2 --zone=us-central1-f --enable-stackdriver-kubernetes'
          }
        }
        stage('Deploy image to GKE cluster using cloud run') {
          steps {
            sh 'gcloud run deploy ${APP_NAME} --cluster-location us-central1-f --image ${IMAGE_TAG} --platform gke --connectivity external --cluster ${APP_NAME}'
          }
        }
        // stage('To call API') {
        //   steps {
        //     sh 'curl -H "Host: ${APP_NAME}.default.example.com" http://kubectl -n gke-system get svc istio-ingress -o jsonpath={.status.loadBalancer.ingress[0].ip}'
        //   }
        // }
    }
}
