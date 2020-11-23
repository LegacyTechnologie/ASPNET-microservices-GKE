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
            sh 'gcloud beta container clusters create ${APP_NAME} --addons=HorizontalPodAutoscaling,HttpLoadBalancing,Istio,CloudRun --machine-type=n1-standard-2 --cluster-version=latest --enable-stackdriver-kubernetes --enable-ip-alias --scopes cloud-platform'
          }
        }
        stage('Get cluster credentials') {
          steps {
            sh 'gcloud container clusters get-credentials ${APP_NAME}'
          }
        }
        stage('Deploy image to GKE cluster using cloud run') {
          steps {
            sh 'gcloud beta run deploy ${APP_NAME} --cluster-location us-central1-f --image ${IMAGE_TAG} --platform gke --connectivity external --cluster ${APP_NAME}'
          }
        }
        stage('Deploy services to expose') {
          steps {
            sh 'kubectl apply -f service.yaml'
          }
        }
        stage('IP or Endpoints') {
          steps {
            sh 'kubectl -n gke-system get svc istio-ingressgateway -o jsonpath="{.status.loadBalancer.ingress[0].ip}"'
          }
        }
    }
}
