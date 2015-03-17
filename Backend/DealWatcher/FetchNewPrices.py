import urllib2

def main():

	resp = urllib2.urlopen("http://dealwatcherservice.azurewebsites.net/api/values")
	content = resp.read()
	resp.close()

	print("Result of API query...")
	print(content)
	

if __name__ == "__main__":
	main()